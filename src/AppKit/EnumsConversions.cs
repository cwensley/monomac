using System;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;

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

