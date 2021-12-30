//
// Copyright 2010, Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

using MonoMac.Foundation;

namespace MonoMac.ObjCRuntime
{
	public static class Runtime
	{
		static List<Assembly> assemblies;
		static Dictionary<IntPtrTypeValueTuple,Delegate> block_to_delegate_cache;
		static Dictionary<IntPtr, WeakReference> object_map = new Dictionary<IntPtr, WeakReference>(IntPtrEqualityComparer);
		static object lock_obj = new object();
		internal static IntPtr selClass = Selector.GetHandle("class");
		internal static readonly IntPtr selDescriptionHandle = Selector.GetHandle ("description");

		internal static IntPtrEqualityComparer IntPtrEqualityComparer;
		internal static TypeEqualityComparer TypeEqualityComparer;
		public static string FrameworksPath
		{
			get; set;
		}

		public static string ResourcesPath
		{
			get; set;
		}
		
		public static readonly bool IsARM64CallingConvention;

		static Runtime()
		{
			// BaseDirectory may not be set in some Mono embedded environments
			// so try some reasonable fallbacks in these cases.
			string basePath = AppDomain.CurrentDomain.BaseDirectory;
			if (!string.IsNullOrEmpty(basePath))
				basePath = Path.Combine(basePath, "..");
			else
			{
				basePath = Assembly.GetExecutingAssembly().Location;
				if (!string.IsNullOrEmpty(basePath))
				{
					basePath = Path.Combine(Path.GetDirectoryName(basePath), "..");
				}
				else
				{
					// The executing assembly location may be null if loaded from
					// memory so the final fallback is the current directory
					basePath = Path.Combine(Environment.CurrentDirectory, "..");
				}
			}

			ResourcesPath = Path.Combine(basePath, "Resources");
			FrameworksPath = Path.Combine(basePath, "Frameworks");
			IsARM64CallingConvention = GetIsARM64CallingConvention();

			IntPtrEqualityComparer = new IntPtrEqualityComparer ();
			TypeEqualityComparer = new TypeEqualityComparer ();
		}
		
		
		private enum NXByteOrder
		{
			Unknown,
			LittleEndian,
			BigEndian
		}		

		private struct NXArchInfo
		{
			private IntPtr name;

			public int CpuType;

			public int CpuSubType;

			public NXByteOrder ByteOrder;

			private IntPtr description;

			public string Name => Marshal.PtrToStringAuto(name);

			public string Description => Marshal.PtrToStringAuto(description);
		}		

		[DllImport("/usr/lib/libSystem.dylib")]
		private unsafe static extern NXArchInfo* NXGetLocalArchInfo();		
		private unsafe static bool GetIsARM64CallingConvention()
		{
			if (IntPtr.Size != 8)
			{
				return false;
			}
			return NXGetLocalArchInfo()->Name.StartsWith("arm64");
		}		

		public static void RegisterAssembly(Assembly a)
		{
			var attributes = a.GetCustomAttributes(typeof(RequiredFrameworkAttribute), false);

			for (int i = 0; i < attributes.Length; i++)
			{
				object attribute = attributes[i];

				var requiredFramework = (RequiredFrameworkAttribute)attribute;
				string libPath;
				string libName = requiredFramework.Name;

				if (libName.Contains(".dylib"))
				{
					libPath = ResourcesPath;
				}
				else
				{
					libPath = FrameworksPath;
					libPath = Path.Combine(libPath, libName);
					libName = libName.Replace(".frameworks", "");
				}
				libPath = Path.Combine(libPath, libName);

				if (Dlfcn.dlopen(libPath, 0) == IntPtr.Zero)
					throw new Exception(string.Format("Unable to load required framework: '{0}'", requiredFramework.Name),
				new Exception(Dlfcn.dlerror()));
			}

			if (assemblies == null)
			{
				assemblies = new List<Assembly>();
				Class.Register(typeof(NSObject));
			}

			assemblies.Add(a);

			Type[] types = a.GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				Type type = types[i];
				if (type.IsSubclassOf(typeof(NSObject)) && !Attribute.IsDefined(type, typeof(ModelAttribute), false))
					Class.Register(type);
			}
		}

		internal static List<Assembly> GetAssemblies()
		{
			if (assemblies == null)
			{
				var this_assembly = typeof(Runtime).Assembly.GetName();
				assemblies = new List<Assembly>();

				Assembly[] domainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0; i < domainAssemblies.Length; i++)
				{
					Assembly a = domainAssemblies[i];
					var refs = a.GetReferencedAssemblies();
					for (int j = 0; j < refs.Length; j++)
					{
						AssemblyName aref = refs[j];
						if (aref == this_assembly)
							assemblies.Add(a);
					}
				}
			}

			return assemblies;
		}

		internal static void UnregisterNSObject(IntPtr ptr)
		{
			lock (lock_obj)
			{
				object_map.Remove(ptr);
			}
		}

		internal static void RegisterNSObject(NSObject obj, IntPtr ptr)
		{
			lock (lock_obj)
			{
				object_map[ptr] = new WeakReference(obj);
				obj.Handle = ptr;
			}
		}

		internal static void NativeObjectHasDied(IntPtr ptr)
		{
			lock (lock_obj)
			{
				WeakReference wr;
				if (object_map.TryGetValue(ptr, out wr))
				{
					object_map.Remove(ptr);

					var obj = (NSObject)wr.Target;
					if (obj != null)
						obj.ClearHandle();
				}
			}
		}

		public static T GetINativeObject<T>(IntPtr ptr)
			where T: INativeObject
		{
			if (TryGetNSObject(ptr) is T nsobj)
				return nsobj;

			var obj = InternalGetNSObject<T>(ptr);
			if (obj == null && ptr != IntPtr.Zero)
			{
				// native object is dead as we are passed a known handle but it is a different type
				// this should theoretically not happen, but it does quite often so this is done
				// for resiliency.
				Debug.WriteLine("Object of type {0} has died but not cleaned up. New type is {1}. Handle: {2}", obj?.GetType(), typeof(T), ptr);

				// kill object in .NET
				NativeObjectHasDied(ptr);

				// re-wrap native handle in a new .NET object of the correct type
				obj = InternalGetNSObject<T>(ptr);
			}
			return obj;
		}

		public static T GetNSObject<T>(IntPtr ptr)
			where T : NSObject
		{
			var obj = TryGetNSObject(ptr) ?? InternalGetNSObject<T>(ptr);
			var result = obj as T;
			if (result == null && ptr != IntPtr.Zero)
			{
				// native object is dead as we are passed a known handle but it is a different type
				// this should theoretically not happen, but it does quite often so this is done
				// for resiliency.
				Debug.WriteLine("Object of type {0} has died but not cleaned up. New type is {1}. Handle: {2}", obj?.GetType(), typeof(T), ptr);

				// kill object in .NET
				NativeObjectHasDied(ptr);

				// re-wrap native handle in a new .NET object of the correct type
				result = InternalGetNSObject<T>(ptr);
			}
			return result;
		}

		public static NSObject TryGetNSObject(IntPtr ptr)
		{
			lock (lock_obj)
			{
				WeakReference reference;
				if (object_map.TryGetValue(ptr, out reference))
					return (NSObject)reference.Target;
			}

			return null;
		}

		public static NSObject GetNSObject(IntPtr ptr)
		{
			return TryGetNSObject(ptr) ?? InternalGetNSObject<NSObject>(ptr);
		}

		static readonly Type[] s_IntPtrTypes = new [] { typeof(IntPtr) };

		static T InternalGetNSObject<T>(IntPtr ptr)
		{
			Type type;

			if (ptr == IntPtr.Zero)
				return default;

			var clsPtr = Messaging.intptr_objc_msgSend(ptr, selClass);
			type = Class.Lookup(clsPtr, false);

			if (type != null && typeof(NSProxy).IsAssignableFrom(type) && !typeof(T).IsAssignableFrom(type))
			{
				// got a proxy, let's attempt to get the real type from the description if it isn't what we are expecting
				// is this the only way to get the concrete underlying class? does it always work?
				type = null;
				var str = NSString.FromHandle(Messaging.intptr_objc_msgSend(ptr, selDescriptionHandle));
				var classIdx = str.IndexOf(' ');
				if (classIdx > 0)
				{
					var clsName = str.Substring(0, classIdx);
					if (!string.IsNullOrEmpty(clsName))
					{
						clsPtr = Class.objc_getClass(clsName);
						if (clsPtr != IntPtr.Zero)
						{
							type = Class.Lookup(clsPtr, false);
							if (type.GetConstructor(s_IntPtrTypes) == null)
							{
								type = null;
							}
						}
					}
				}
			}

			if (type == null)
				type = typeof(T);

			return (T)Activator.CreateInstance(type, new object[] { ptr });
		}


		public static void ConnectMethod(MethodInfo method, Selector selector)
		{
			if (method == null)
				throw new ArgumentNullException("method");
			if (selector == null)
				throw new ArgumentNullException("selector");
			var type = method.DeclaringType;

			if (!Class.IsCustomType(type))
				throw new ArgumentException("Cannot late bind methods on core types");

			var ea = new ExportAttribute(selector.Name);
			var klass = new Class(type);

			Class.RegisterMethod(method, ea, type, klass.Handle);
		}

		internal static Delegate GetDelegateForBlock (IntPtr methodPtr, Type type)
		{
			// We do not care if there is a race condition and we initialize two caches
			// since the worst that can happen is that we end up with an extra
			// delegate->function pointer.
			Delegate val;
			var pair = new IntPtrTypeValueTuple (methodPtr, type);
			lock (lock_obj) {
				if (block_to_delegate_cache == null)
					block_to_delegate_cache = new Dictionary<IntPtrTypeValueTuple, Delegate> ();

				if (block_to_delegate_cache.TryGetValue (pair, out val))
					return val;
			}

			val = Marshal.GetDelegateForFunctionPointer (methodPtr, type);

			lock (lock_obj) {
				block_to_delegate_cache [pair] = val;
			}
			return val;
		}
	}

	internal struct IntPtrTypeValueTuple : IEquatable<IntPtrTypeValueTuple>
	{
		static readonly IEqualityComparer<IntPtr> item1Comparer = Runtime.IntPtrEqualityComparer;
		static readonly IEqualityComparer<Type> item2Comparer = Runtime.TypeEqualityComparer;

		public readonly IntPtr Item1;
		public readonly Type Item2;

		public IntPtrTypeValueTuple (IntPtr item1, Type item2)
		{
			Item1 = item1;
			Item2 = item2;
		}

		public bool Equals (IntPtrTypeValueTuple other)
		{
			return item1Comparer.Equals (Item1, other.Item1) &&
				item2Comparer.Equals (Item2, other.Item2);
		}

		public override bool Equals (object obj)
		{
			if (obj is IntPtrTypeValueTuple)
				return Equals ((IntPtrTypeValueTuple)obj);

			return false;
		}

		public override int GetHashCode ()
		{
			return item1Comparer.GetHashCode (Item1) ^ item2Comparer.GetHashCode (Item2);
		}

		public static bool operator == (IntPtrTypeValueTuple left, IntPtrTypeValueTuple right)
		{
			return left.Equals(right);
		}

		public static bool operator != (IntPtrTypeValueTuple left, IntPtrTypeValueTuple right)
		{
			return !left.Equals(right);
		}
	}
}
