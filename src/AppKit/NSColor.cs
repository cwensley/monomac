using System;
using System.Text;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using System.Runtime.InteropServices;

#if MAC64
using nint = System.Int64;
using nuint = System.UInt64;
using nfloat = System.Double;
#else
using nint = System.Int32;
using nuint = System.UInt32;
using nfloat = System.Single;
#if SDCOMPAT
using CGPoint = System.Drawing.PointF;
using CGSize = System.Drawing.SizeF;
using CGRect = System.Drawing.RectangleF;
#endif
#endif

namespace MonoMac.AppKit {
	public partial class NSColor {

		public static NSColor FromColorSpace (NSColorSpace space, nfloat[] components)
		{
			if (components == null)
				throw new ArgumentNullException ("components");

			int size = Marshal.SizeOf(components[0]) * components.Length;
			IntPtr pNativeFloatArray = Marshal.AllocHGlobal(size);
			Marshal.Copy(components, 0, pNativeFloatArray, components.Length);

			NSColor color = _FromColorSpace (space, pNativeFloatArray, components.Length);

			Marshal.FreeHGlobal(pNativeFloatArray);

			return color;
		}
		
		public void GetComponents(out nfloat[] components)
		{
			int count = (int)this.ComponentCount;
			nfloat[] managedFloatArray = new nfloat[count];
			int size = Marshal.SizeOf(managedFloatArray[0]) * count;
			IntPtr pNativeFloatArray = Marshal.AllocHGlobal(size);

			_GetComponents (pNativeFloatArray);
			Marshal.Copy(pNativeFloatArray, managedFloatArray, 0, count);
			Marshal.FreeHGlobal(pNativeFloatArray);

			components = managedFloatArray;
		}

		public override string ToString ()
		{
			try {
				string name = this.ColorSpaceName;
				if (name == "NSNamedColorSpace")
					return this.LocalizedCatalogNameComponent +"/" + this.LocalizedColorNameComponent;
				if (name == "NSPatternColorSpace")
					return "Pattern Color: " + this.PatternImage.Name;
				
				StringBuilder sb = new StringBuilder (this.ColorSpace.LocalizedName);
				nfloat[] components;
				this.GetComponents (out components);
				if (components.Length > 0)
					sb.Append ("(" + components [0]);
				for (int i = 1; i < components.Length; i++)
					sb.Append ("," + components [i]);
				sb.Append (")");
				
				return sb.ToString ();
			} catch {
				//fallback to base method if we have an unexpected condition.
				return base.ToString ();
			}
		}
	}
}

