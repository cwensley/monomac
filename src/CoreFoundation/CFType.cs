//
// Copyright 2012 Xamarin
//
using System;
using System.Runtime.InteropServices;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreFoundation {
	public class CFType {
		[DllImport (Constants.CoreFoundationLibrary, EntryPoint="CFGetTypeID")]
		public static extern int GetTypeID (IntPtr typeRef);

		[DllImport (Constants.CoreFoundationLibrary)]
		extern static IntPtr CFCopyDescription (IntPtr ptr);

		public string GetDescription (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentNullException ("handle");

			return CFString.FromHandle(CFCopyDescription(handle));
		}
		
	}

	public interface ICFType : INativeObject {
	}
}
