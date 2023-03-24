using System;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

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

namespace MonoMac.AppKit
{
	static class NSTextAlignmentConversions
	{
		public static nuint ToNative(NSTextAlignment value)
		{
			if (Runtime.IsARM64CallingConvention)
			{
				// Right/Center are switched on apple silicon
				switch (value)
				{
					case NSTextAlignment.Right:
						value = NSTextAlignment.Center;
						break;
					case NSTextAlignment.Center:
						value = NSTextAlignment.Right;
						break;
				}
			}
			return (nuint)(ulong)value;
		}

		public static NSTextAlignment ToManaged(nuint value)
		{
			return (NSTextAlignment)(ulong)ToNative((NSTextAlignment)(ulong)value);
		}
	}
}

