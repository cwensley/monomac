using System;

namespace MonoMac.ObjCRuntime {
	public interface INativeObject {
		IntPtr Handle { get; }
	}

	public static class NativeObjectExtensions {

		// help to avoid the (too common pattern)
		// 	var p = x == null ? IntPtr.Zero : x.Handle;
		static public IntPtr GetHandle (this INativeObject self)
		{
			return self == null ? IntPtr.Zero : self.Handle;
		}

		static public IntPtr GetNonNullHandle (this INativeObject self, string argumentName)
		{
			if (self == null)
				throw new ArgumentNullException(nameof(argumentName));
			if (self.Handle == IntPtr.Zero)
				throw new ObjectDisposedException(self.GetType().ToString());
			return self.Handle;
		}
	}
}
