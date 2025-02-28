//
// Copyright 2010, 2011 Novell, Inc.
// Copyright 2011, Xamarin, Inc.
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

namespace MonoMac.AppKit {
	
	public enum NSRunResponse : nint {
		Stopped = -1000,
		Aborted = -1001,
		Continues = -1002
	}

	public enum NSApplicationActivationOptions : nuint {
		ActivateAllWindows = 1,
		ActivateIgnoringOtherWindows = 2
	}
	
	public enum NSApplicationActivationPolicy : nint {
		Regular, Accessory, Prohibited
	}
	
	public enum NSApplicationPresentationOptions : nuint {
		Default                    = 0,
		AutoHideDock               = (1 <<  0),
		HideDock                   = (1 <<  1),

		AutoHideMenuBar            = (1 <<  2),
		HideMenuBar                = (1 <<  3),

		DisableAppleMenu           = (1 <<  4),
		DisableProcessSwitching    = (1 <<  5),
		DisableForceQuit           = (1 <<  6),
		DisableSessionTermination  = (1 <<  7),
		DisableHideApplication     = (1 <<  8),
		DisableMenuBarTransparency = (1 <<  9),

		FullScreen                 = (1 << 10),
		AutoHideToolbar            = (1 << 11)
	}

	public enum NSApplicationDelegateReply : nuint {
		Success,
		Cancel,
		Failure
	}

	public enum NSRequestUserAttentionType : nuint {
		CriticalRequest = 0,
		InformationalRequest = 10
	}

	public enum NSApplicationTerminateReply : nuint {
		Cancel, Now, Later
	}

	public enum NSApplicationPrintReply : nuint {
		Cancelled, Success, Failure, ReplyLater
	}

	public enum NSApplicationLayoutDirection : nint {
		LeftToRight = 0,
		RightToLeft = 1
	}

	public enum NSImageInterpolation : nuint {
		Default, None, Low, Medium, High
	}

	public enum NSComposite : nuint {
		Clear,
		Copy,
		SourceOver,
		SourceIn,
		SourceOut,
		SourceAtop,
		DestinationOver,
		DestinationIn,
		DestinationOut,
		DestinationAtop,
		XOR,
		PlusDarker,
		Highlight,
		PlusLighter,
	}

	public enum NSBackingStore : nuint {
		Retained, Nonretained, Buffered
	}

	public enum NSWindowOrderingMode : nint {
		Below = -1, Out, Above
	}

	public enum NSFocusRingPlacement : nuint {
		RingOnly, RingBelow, RingAbove,
	}

	public enum NSFocusRingType : nuint {
		Default, None, Exterior
	}
	
	public enum NSColorRenderingIntent : nint {
		Default,
		AbsoluteColorimetric,
		RelativeColorimetric,
		Perceptual,
		Saturation
		
	}

	public enum NSRectEdge {
		MinXEdge, MinYEdge, MaxXEdge, MaxYEdge
	}

	public enum NSUserInterfaceLayoutDirection : nint {
		LeftToRight, RightToLeft
	}

#region NSColorSpace
	public enum NSColorSpaceModel : nint {
		Unknown = -1,
		Gray,
		RGB,
		CMYK,
		LAB,
		DeviceN,
		Indexed,
		Pattern
    }
#endregion

#region NSFileWrapper
	[Flags]
	public enum NSFileWrapperReadingOptions : nuint {
		Immediate = 1, WithoutMapping = 2
	}
#endregion
	
#region NSParagraphStyle
	public enum NSTextTabType : nuint {
		Left, Right, Center, Decimal
	}

	public enum NSLineBreakMode : nuint {
		ByWordWrapping,
		CharWrapping,
		Clipping,
		TruncatingHead,
		TruncatingTail,
		TruncatingMiddle
	}
	
#endregion
	
#region NSCell Defines 

	public enum NSType : nuint {
	    Any			= 0,
	    Int			= 1,
	    PositiveInt		= 2,
	    Float		= 3,
	    PositiveFloat	= 4,
	    Double		= 6,
	    PositiveDouble	= 7
	}
	
	public enum NSCellType : nuint {
	    Null,
	    Text,
	    Image
	}
	
	public enum NSCellAttribute : nuint {
		CellDisabled,
		CellState,
		PushInCell,
		CellEditable,
		ChangeGrayCell,
		CellHighlighted,
		CellLightsByContents,
		CellLightsByGray,
		ChangeBackgroundCell,
		CellLightsByBackground,
		CellIsBordered,
		CellHasOverlappingImage,
		CellHasImageHorizontal,
		CellHasImageOnLeftOrBottom,
		CellChangesContents,
		CellIsInsetButton,
		CellAllowsMixedState,
	}
	
	public enum NSCellImagePosition : nuint {
		NoImage,
		ImageOnly,
		ImageLeft,
		ImageRight,
		ImageBelow,
		ImageAbove,
		ImageOverlaps,
	}
	
	public enum NSImageScale : nuint {
		ProportionallyDown = 0,
		AxesIndependently,     
		None,                 
		ProportionallyUpOrDown
	}
	
	public enum NSCellStateValue : nint {
		Mixed = -1,
		Off,
		On
	}
	
	[Flags]
	public enum NSCellMask : nuint {
		NoCell = 0,
		ContentsCell = 1 << 0,
		PushInCell = 1 << 1, 
		ChangeGrayCell = 1 << 2,
		ChangeBackgroundCell = 1 << 3
	}
	
	[Flags]
	public enum NSCellHit : nuint {
		None,
		ContentArea = 1,
		EditableTextArea = 2,
		TrackableArae = 4
	}
	
	public enum NSControlTint : nuint {
		Default  = 0,	// system 'default'
		Blue     = 1,
		Graphite = 6,
		Clear    = 7
	}
	
	public enum NSControlSize : nuint {
		Regular, 
		Small,
		Mini
	}

	public enum NSBackgroundStyle : nint {
		Light, Dark, Raised, Lowered
	}
#endregion

#region NSImage
	
	public enum NSImageLoadStatus : nuint {
	    		Completed,
	    		Cancelled,
	    		InvalidData,
	    		UnexpectedEOF,
	    		ReadError
	}
	
	public enum NSImageCacheMode : nuint {
		Default, 
		Always,  
		BySize,  
		Never    
	}
		
#endregion
	
#region NSAlert
	public enum NSAlertStyle : nuint {
		Warning, Informational, Critical
	}
	
	[Mac (10,9)]
	public enum NSModalResponse : long {
		OK = 1,
		Cancel = 0,
		Stop = -1000,
		Abort = -1001,
		Continue = -1002
	}
	
#endregion

#region NSEvent
	public enum NSEventType : nuint {
		LeftMouseDown = 1,
		LeftMouseUp = 2,
		RightMouseDown = 3,
		RightMouseUp = 4,
		MouseMoved = 5,
		LeftMouseDragged = 6,
		RightMouseDragged = 7,
		MouseEntered = 8,
		MouseExited = 9,
		KeyDown = 10,
		KeyUp = 11,
		FlagsChanged = 12,
		AppKitDefined = 13,
		SystemDefined = 14,
		ApplicationDefined = 15,
		Periodic = 16,
		CursorUpdate = 17,

		ScrollWheel = 22,

		TabletPoint = 23,
		TabletProximity = 24,

		OtherMouseDown = 25,
		OtherMouseUp = 26,
		OtherMouseDragged = 27,

		Gesture = 29,
		Magnify = 30,
		Swipe = 31,
		Rotate = 18,
		BeginGesture = 19,
		EndGesture = 20,

		SmartMagnify = 32,
		QuickLook = 33
	}

	[Flags]
	public enum NSEventMask : ulong {
		LeftMouseDown         = 1 << (int)NSEventType.LeftMouseDown,
		LeftMouseUp           = 1 << (int)NSEventType.LeftMouseUp,
		RightMouseDown        = 1 << (int)NSEventType.RightMouseDown,
		RightMouseUp          = 1 << (int)NSEventType.RightMouseUp,
		MouseMoved            = 1 << (int)NSEventType.MouseMoved,
		LeftMouseDragged      = 1 << (int)NSEventType.LeftMouseDragged,
		RightMouseDragged     = 1 << (int)NSEventType.RightMouseDragged,
		MouseEntered          = 1 << (int)NSEventType.MouseEntered,
		MouseExited           = 1 << (int)NSEventType.MouseExited,
		KeyDown               = 1 << (int)NSEventType.KeyDown,
		KeyUp                 = 1 << (int)NSEventType.KeyUp,
		FlagsChanged          = 1 << (int)NSEventType.FlagsChanged,
		AppKitDefined         = 1 << (int)NSEventType.AppKitDefined,
		SystemDefined         = 1 << (int)NSEventType.SystemDefined,
		ApplicationDefined    = 1 << (int)NSEventType.ApplicationDefined,
		Periodic              = 1 << (int)NSEventType.Periodic,
		CursorUpdate          = 1 << (int)NSEventType.CursorUpdate,
		ScrollWheel           = 1 << (int)NSEventType.ScrollWheel,
		TabletPoint           = 1 << (int)NSEventType.TabletPoint,
		TabletProximity       = 1 << (int)NSEventType.TabletProximity,
		OtherMouseDown        = 1 << (int)NSEventType.OtherMouseDown,
		OtherMouseUp          = 1 << (int)NSEventType.OtherMouseUp,
		OtherMouseDragged     = 1 << (int)NSEventType.OtherMouseDragged,
		EventGesture          = 1 << (int)NSEventType.Gesture,
		EventMagnify          = (uint)1 << (int)NSEventType.Magnify,
		EventSwipe            = (uint)1 << (int)NSEventType.Swipe,
		EventRotate           = (uint)1 << (int)NSEventType.Rotate,
		EventBeginGesture     = (uint)1 << (int)NSEventType.BeginGesture,
		EventEndGesture       = (uint)1 << (int)NSEventType.EndGesture,
		AnyEvent              = UInt64.MaxValue
	}

	[Flags]
	public enum NSEventModifierMask : nuint {
		AlphaShiftKeyMask         = 1 << 16,
		ShiftKeyMask              = 1 << 17,
		ControlKeyMask            = 1 << 18,
		AlternateKeyMask          = 1 << 19,
		CommandKeyMask            = 1 << 20,
		NumericPadKeyMask         = 1 << 21,
		HelpKeyMask               = 1 << 22,
		FunctionKeyMask           = 1 << 23,
		DeviceIndependentModifierFlagsMask    = 0xffff0000
	}

	public enum NSPointingDeviceType : nuint {
		Unknown, Pen, Cursor, Eraser
	}

	[Flags]
	public enum NSPointingDeviceMask {
		Pen = 1, PenLower = 2, PenUpper = 4
	}

	public enum NSKey : nuint {
		A              = 0x00,
		S              = 0x01,
		D              = 0x02,
		F              = 0x03,
		H              = 0x04,
		G              = 0x05,
		Z              = 0x06,
		X              = 0x07,
		C              = 0x08,
		V              = 0x09,
		B              = 0x0B,
		Q              = 0x0C,
		W              = 0x0D,
		E              = 0x0E,
		R              = 0x0F,
		Y              = 0x10,
		T              = 0x11,
		D1             = 0x12,
		D2             = 0x13,
		D3             = 0x14,
		D4             = 0x15,
		D6             = 0x16,
		D5             = 0x17,
		Equal          = 0x18,
		D9             = 0x19,
		D7             = 0x1A,
		Minus          = 0x1B,
		D8             = 0x1C,
		D0             = 0x1D,
		RightBracket   = 0x1E,
		O              = 0x1F,
		U              = 0x20,
		LeftBracket    = 0x21,
		I              = 0x22,
		P              = 0x23,
		L              = 0x25,
		J              = 0x26,
		Quote          = 0x27,
		K              = 0x28,
		Semicolon      = 0x29,
		Backslash      = 0x2A,
		Comma          = 0x2B,
		Slash          = 0x2C,
		N              = 0x2D,
		M              = 0x2E,
		Period         = 0x2F,
		Grave          = 0x32,
		KeypadDecimal  = 0x41,
		KeypadMultiply = 0x43,
		KeypadPlus     = 0x45,
		KeypadClear    = 0x47,
		KeypadDivide   = 0x4B,
		KeypadEnter    = 0x4C,
		KeypadMinus    = 0x4E,
		KeypadEquals   = 0x51,
		Keypad0        = 0x52,
		Keypad1        = 0x53,
		Keypad2        = 0x54,
		Keypad3        = 0x55,
		Keypad4        = 0x56,
		Keypad5        = 0x57,
		Keypad6        = 0x58,
		Keypad7        = 0x59,
		Keypad8        = 0x5B,
		Keypad9        = 0x5C,
		Return         = 0x24,
		Tab            = 0x30,
		Space          = 0x31,
		Escape         = 0x35,
		Command        = 0x37,
		Shift          = 0x38,
		CapsLock       = 0x39,
		Option         = 0x3A,
		Control        = 0x3B,
		RightShift     = 0x3C,
		RightOption    = 0x3D,
		RightControl   = 0x3E,
		Function       = 0x3F,
		VolumeUp       = 0x48,
		VolumeDown     = 0x49,
		Mute           = 0x4A,
		ForwardDelete  = 0x75,
		UpArrow        = 0xF700,
		DownArrow      = 0xF701,
		LeftArrow      = 0xF702,
		RightArrow     = 0xF703,
		F1             = 0xF704,
		F2             = 0xF705,
		F3             = 0xF706,
		F4             = 0xF707,
		F5             = 0xF708,
		F6             = 0xF709,
		F7             = 0xF70A,
		F8             = 0xF70B,
		F9             = 0xF70C,
		F10            = 0xF70D,
		F11            = 0xF70E,
		F12            = 0xF70F,
		F13            = 0xF710,
		F14            = 0xF711,
		F15            = 0xF712,
		F16            = 0xF713,
		F17            = 0xF714,
		F18            = 0xF715,
		F19            = 0xF716,
		F20            = 0xF717,
		F21            = 0xF718,
		F22            = 0xF719,
		F23            = 0xF71A,
		F24            = 0xF71B,
		F25            = 0xF71C,
		F26            = 0xF71D,
		F27            = 0xF71E,
		F28            = 0xF71F,
		F29            = 0xF720,
		F30            = 0xF721,
		F31            = 0xF722,
		F32            = 0xF723,
		F33            = 0xF724,
		F34            = 0xF725,
		F35            = 0xF726,
		Insert         = 0xF727,
		Delete         = 51,
		Home           = 0xF729,
		Begin          = 0xF72A,
		End            = 0xF72B,
		PageUp         = 0xF72C,
		PageDown       = 0xF72D,
		PrintScreen    = 0xF72E,
		ScrollLock     = 0xF72F,
		Pause          = 0xF730,
		SysReq         = 0xF731,
		Break          = 0xF732,
		Reset          = 0xF733,
		Stop           = 0xF734,
		Menu           = 0xF735,
		User           = 0xF736,
		System         = 0xF737,
		Print          = 0xF738,
		ClearLine      = 0xF739,
		ClearDisplay   = 0xF73A,
		InsertLine     = 0xF73B,
		DeleteLine     = 0xF73C,
		InsertChar     = 0xF73D,
		DeleteChar     = 0xF73E,
		Prev           = 0xF73F,
		Next           = 0xF740,
		Select         = 0xF741,
		Execute        = 0xF742,
		Undo           = 0xF743,
		Redo           = 0xF744,
		Find           = 0xF745,
		Help           = 0xF746,
		ModeSwitch     = 0xF747
	}

	public enum NSEventSubtype : nuint {
		WindowExposed = 0,
		ApplicationActivated = 1,
		ApplicationDeactivated = 2,
		WindowMoved = 4,
		ScreenChanged = 8,
		AWT = 16
	}

	public enum NSSystemDefinedEvents : nuint {
		NSPowerOffEventType = 1
	}

	public enum NSEventMouseSubtype {
		Mouse, TablePoint, TabletProximity, Touch
	}
	
#endregion

#region NSView
	[Flags]
	public enum NSViewResizingMask : nuint {
		NotSizable		=  0,
		MinXMargin		=  1,
		WidthSizable		=  2,
		MaxXMargin		=  4,
		MinYMargin		=  8,
		HeightSizable		= 16,
		MaxYMargin		= 32
	}
	
	public enum NSBorderType : nuint {
		NoBorder, LineBorder, BezelBorder, GrooveBorder
	}

	public enum NSTextFieldBezelStyle {
		Square, Rounded
	}
	
	public enum NSViewLayerContentsRedrawPolicy : nint {
		Never, OnSetNeedsDisplay, DuringViewResize, BeforeViewResize
	}

	public enum NSViewLayerContentsPlacement : nint {
		ScaleAxesIndependently,
		ScaleProportionallyToFit,
		ScaleProportionallyToFill,
		Center,
		Top,
		TopRight,
		Right,
		BottomRight,
		Bottom,
		BottomLeft,
		Left,
		TopLeft,
	}

#endregion
	
#region NSWindow
	[Flags]
	public enum NSWindowStyle : nuint {
		Borderless	       					= 0 << 0,
		Titled		       					= 1 << 0,
		Closable	       					= 1 << 1,
		Miniaturizable	      				= 1 << 2,
		Resizable	       					= 1 << 3,
		Utility		       					= 1 << 4,
		DocModal	       					= 1 << 6,
		NonactivatingPanel     				= 1 << 7,
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Don't use 'TexturedBackground' anymore.")]
		TexturedBackground     				= 1 << 8,
#if !NET
		[Deprecated (PlatformName.MacOSX, 10, 9, message: "Don't use, this value has no effect.")]
		Unscaled	       					= 1 << 11,
#endif
		UnifiedTitleAndToolbar 				= 1 << 12,
		Hud		       						= 1 << 13,
		FullScreenWindow       				= 1 << 14,
		[Mac (10,10)] FullSizeContentView   = 1 << 15 
	}

	public enum NSWindowSharingType : nuint {
		None, ReadOnly, ReadWrite
	}

	public enum NSWindowBackingLocation : nuint {
		Default, VideoMemory, MainMemory,
	}

	[Flags]
	public enum NSWindowCollectionBehavior : nuint {
		Default = 0,
		CanJoinAllSpaces = 1 << 0,
		MoveToActiveSpace = 1 << 1,
		Managed = 1 << 2,
		Transient = 1 << 3,
		Stationary = 1 << 4,
		ParticipatesInCycle = 1 << 5,
		IgnoresCycle = 1 << 6,
		FullScreenPrimary = 1 << 7,
		FullScreenAuxiliary = 1 << 8,
		
		[ElCapitan]
		FullScreenAllowsTiling = 1 << 11,
		[ElCapitan]
		FullScreenDisallowsTiling = 1 << 12
	}

	public enum NSWindowNumberListOptions : nuint {
		AllApplication = 1 << 0,
		AllSpaces = 1 << 4
	}

	public enum NSSelectionDirection : nuint {
		Direct = 0,
		Next,
		Previous
	}

	public enum NSWindowButton : nuint {
		CloseButton, MiniaturizeButton, ZoomButton, ToolbarButton, DocumentIconButton, DocumentVersionsButton = 6, FullScreenButton
	}

	public enum NSTouchPhase : nuint {
		Began           = 1 << 0,
		Moved           = 1 << 1,
		Stationary      = 1 << 2,
		Ended           = 1 << 3,
		Cancelled       = 1 << 4,
		
		Touching        = Began | Moved | Stationary,
		Any             = nuint.MaxValue
	}
#endregion
#region NSAnimation
	
	public enum NSAnimationCurve : nuint {
		EaseInOut,
		EaseIn,
		EaseOut,
		Linear
	};
	
	public enum NSAnimationBlockingMode : nuint {
		Blocking,
		Nonblocking,
		NonblockingThreaded
	};
#endregion

#region NSBox
	
	public enum NSTitlePosition {
		NoTitle,
		AboveTop,
		AtTop,
		BelowTop,
		AboveBottom,
		AtBottom,
		BelowBottom
	};

	public enum NSBoxType {
		NSBoxPrimary,
		NSBoxSecondary,
		NSBoxSeparator,
		NSBoxOldStyle,
		NSBoxCustom
	};
#endregion

#region NSButtonCell
	public enum NSButtonType : nuint {
		MomentaryLightButton,
		PushOnPushOff,
		Toggle,
		Switch,
		Radio,
		MomentaryChange,
		OnOff,
		MomentaryPushIn
	}
	
	public enum NSBezelStyle : nuint {
		Rounded = 1,
		RegularSquare,
		ThickSquare,
		ThickerSquare,
		Disclosure,
		ShadowlessSquare,
		Circular,
		TexturedSquare,
		HelpButton,
		SmallSquare,
		TexturedRounded,
		RoundRect,
		Recessed,
		RoundedDisclosure,
		Inline
	}

	public enum NSGradientType : nuint {
		None,
		ConcaveWeak,
		ConcaveStrong,
		ConvexWeak,
		ConvexStrong
	}
	
#endregion

#region NSGraphics
	public enum NSWindowDepth : nint {
		TwentyfourBitRgb = 0x208,
		SixtyfourBitRgb = 0x210,
		OneHundredTwentyEightBitRgb = 0x220	
	}

	public enum NSCompositingOperation : nuint {
		Clear,
		Copy,
		SourceOver,
		SourceIn,
		SourceOut,
		SourceAtop,
		DestinationOver,
		DestinationIn,
		DestinationOut,
		DestinationAtop,
		Xor,
		PlusDarker,
		Highlight,
		PlusLighter,
	}

	public enum NSAnimationEffect : nuint {
		DissapearingItemDefault = 0,
		EffectPoof = 10
	}
#endregion

#region NSMatrix
	public enum NSMatrixMode {
		Radio, Highlight, List, Track
	}
#endregion

#region NSBrowser
	public enum NSBrowserColumnResizingType : nuint {
		None, Auto, User
	}

	public enum NSBrowserDropOperation : nuint {
		On, Above
	}
#endregion

#region NSColorPanel
	public enum NSColorPanelMode : nint {
		None = -1,
		Gray = 0,
		RGB,
		CMYK,
		HSB,
		CustomPalette,
		ColorList,
		Wheel,
		Crayon
	};

	[Flags]
	public enum NSColorPanelFlags {
	    Gray	= 0x00000001,
	    RGB		= 0x00000002,
	    CMYK	= 0x00000004,
	    HSB		= 0x00000008,
	    CustomPalette= 0x00000010,
	    ColorList	= 0x00000020,
	    Wheel	= 0x00000040,
	    Crayon	= 0x00000080,
	    All		= 0x0000ffff
	}
#endregion
#region NSDocument

	public enum NSDocumentChangeType : nuint {
		Done, Undone, Cleared, ReadOtherContents, Autosaved, Redone,
		Discardable = 256 /* New in Lion */
	}

	public enum NSSaveOperationType : nuint  {
		Save, SaveAs, SaveTo,
		Autosave = 3,	/* Deprecated name in Lion */
		Elsewhere = 3,	/* New Lion name */
		InPlace = 4,	/* New in Lion */
		AutoSaveAs = 5	/* New in Mountain Lion */
	}

#endregion

#region NSBezelPath
	
	public enum NSLineCapStyle {
		Butt, Round, Square
	}
	
	public enum NSLineJoinStyle {
		Miter, Round, Bevel
	}
	
	public enum NSWindingRule {
		NonZero, EvenOdd
	}
	
	public enum NSBezierPathElement {
		MoveTo, LineTo, CurveTo, ClosePath
	}
#endregion

#region NSRulerView
	public enum NSRulerOrientation {
		Horizontal, Vertical
	}
#endregion
	
	[Flags]
	public enum NSDragOperation : nuint {
		None,
		Copy = 1,
		Link = 2,
		Generic = 4,
		Private = 8,
		AllObsolete = 15,
		Move = 16,
		Delete = 32,
		All = nuint.MaxValue
	}

	[Native(ConvertToNative = "NSTextAlignmentConversions.ToNative", ConvertToManaged = "NSTextAlignmentConversions.ToManaged")]
	public enum NSTextAlignment : nuint {
		Left, Right, Center, Justified, Natural
	}
	
	[Flags]
	public enum NSWritingDirection : nint {
		Natural = -1, LeftToRight, RightToLeft,
		Embedding = 0,
		Override = 2,
	}

	public enum NSTextMovement : nuint {
		Other = 0,
		Return = 0x10,
		Tab = 0x11,
		Backtab = 0x12,
		Left = 0x13,
		Right = 0x14,
		Up = 0x15,
		Down = 0x16,
		Cancel = 0x17
	}
	
	[Flags]
	public enum NSMenuProperty : nuint {
		Title = 1 << 0,
		AttributedTitle = 1 << 1,
		KeyEquivalent = 1 << 2,
		Image = 1 << 3,
		Enabled = 1 << 4,
		AccessibilityDescription = 1 << 5
	}

	public enum NSFontRenderingMode : nuint {
		Default, Antialiased, IntegerAdvancements, AntialiasedIntegerAdvancements
	}

	[Flags]
	public enum NSPasteboardReadingOptions : nuint {
		AsData = 0,
		AsString = 1,
		AsPropertyList = 2,
		AsKeyedArchive = 4
	}

	public enum NSUnderlineStyle {
		None                = 0x00,
		Single              = 0x01,
		Thick               = 0x02,
		Double              = 0x09
	}

	public enum NSUnderlinePattern {
		Solid             = 0x0000,
		Dot               = 0x0100,
		Dash              = 0x0200,
		DashDot           = 0x0300,
		DashDotDot        = 0x0400
	}

	public enum NSSelectionAffinity : nuint {
		Upstream, Downstream
	}

	public enum NSSelectionGranularity : nuint {
		Character, Word, Paragraph
	}

#region NSTrackingArea
	[Flags]
	public enum NSTrackingAreaOptions : nuint {
		MouseEnteredAndExited     = 0x01,
		MouseMoved                = 0x02,
		CursorUpdate 		  = 0x04,
		ActiveWhenFirstResponder  = 0x10,
		ActiveInKeyWindow         = 0x20,
		ActiveInActiveApp 	  = 0x40,
		ActiveAlways 		  = 0x80,
		AssumeInside              = 0x100,
		InVisibleRect             = 0x200,
		EnabledDuringMouseDrag    = 0x400 	
	}
#endregion

	public enum NSLineSweepDirection : nuint {
		NSLineSweepLeft,
		NSLineSweepRight,
		NSLineSweepDown,
		NSLineSweepUp
	}

	public enum NSLineMovementDirection : nuint {
		None, Left, Right, Down, Up
	}

	public enum  NSTiffCompression : nuint {
		None = 1,
		CcittFax3 = 3,
		CcittFax4 = 4,
		Lzw = 5,

		[Obsolete ("no longer supported")]
		Jpeg		= 6,
		Next		= 32766,
		PackBits	= 32773,

		[Obsolete ("no longer supported")]
		OldJpeg		= 32865
	}

	public enum NSBitmapImageFileType : nuint {
		Tiff,
		Bmp,
		Gif,
		Jpeg,
		Png,
		Jpeg2000
	}

	public enum NSImageRepLoadStatus : nint {
		UnknownType     = -1,
		ReadingHeader   = -2,
		WillNeedAllData = -3,
		InvalidData     = -4,
		UnexpectedEOF   = -5,
		Completed       = -6 
	}

	[Flags]
	public enum NSBitmapFormat : nuint {
        AlphaFirst                  = 1 << 0,
        AlphaNonpremultiplied       = 1 << 1,
        FloatingPointSamples        = 1 << 2,
   
        LittleEndian16Bit           = 1 << 8,
        LittleEndian32Bit           = 1 << 9,
        BigEndian16Bit              = 1 << 10,
        BigEndian32Bit              = 1 << 11
    }

	public enum NSPrintingOrientation : nuint {
		Portrait, Landscape
	}
	
	public enum NSPrintingPaginationMode : nuint {
		Auto, Fit, Clip
	}

	[Flags]
	public enum NSGlyphStorageOptions : nuint {
		ShowControlGlyphs = 1,
		ShowInvisibleGlyphs = 2,
		WantsBidiLevels = 4
	}

	[Flags]
	public enum NSTextStorageEditedFlags : nuint {
		EditedAttributed = 1,
		EditedCharacters = 2
	}

	public enum NSPrinterTableStatus : nuint {
		Ok, NotFound, Error
	}

	public enum NSScrollArrowPosition : nuint {
		MaxEnd, MinEnd, DefaultSetting, None
	}

	public enum NSUsableScrollerParts : nuint {
		NoScroller, OnlyArrows, All
	}

	public enum NSScrollerPart : nuint {
		None, DecrementPage, Knob, IncrementPage, DecrementLine, IncrementLine, KnobSlot
	}

	public enum NSScrollerArrow : nuint {
		IncrementArrow, DecrementArrow
	}

	public enum NSPrintingPageOrder : nint {
		Descending = -1,
		Special,
		Ascending,
		Unknown
	}

	[Flags]
	public enum NSPrintPanelOptions : nuint {
		ShowsCopies = 1,
		ShowsPageRange = 2,
		ShowsPaperSize = 4,
		ShowsOrientation = 8,
		ShowsScaling = 16,
		ShowsPrintSelection = 32,
		ShowsPageSetupAccessory = 256,
		ShowsPreview = 131072
	}

	public enum NSTextBlockValueType : nuint {
		Absolute, Percentage
	}

	public enum NSTextBlockDimension : nuint {
		Width, MinimumWidth, MaximumWidth, Height, MinimumHeight, MaximumHeight
	}
	
	public enum NSTextBlockLayer : nint {
		Padding = -1, Border, Margin
	}

	public enum NSTextBlockVerticalAlignment : nuint {
		Top, Middle, Bottom, Baseline
	}

	public enum NSTextTableLayoutAlgorithm : nuint {
		Automatic, Fixed
	}

	[Flags]
	public enum NSTextListOptions : nuint {
		PrependEnclosingMarker = 1
	}

	[Flags]
	public enum NSFontSymbolicTraits : int {
		ItalicTrait = (1 << 0),
		BoldTrait = (1 << 1),
		ExpandedTrait = (1 << 5),
		CondensedTrait = (1 << 6),
		MonoSpaceTrait = (1 << 10),
		VerticalTrait = (1 << 11), 
		UIOptimizedTrait = (1 << 12),
		
		UnknownClass = 0 << 28,
		OldStyleSerifsClass = 1 << 28,
		TransitionalSerifsClass = 2 << 28,
		ModernSerifsClass = 3 << 28,
		ClarendonSerifsClass = 4 << 28,
		SlabSerifsClass = 5 << 28,
		FreeformSerifsClass = 7 << 28,
		SansSerifClass = 8 << 28,
		OrnamentalsClass = 9 << 28,
		ScriptsClass = 10 << 28,
		SymbolicClass = 12 << 28,

		FamilyClassMask = (int) -268435456,
	}

	[Flags]
	public enum NSFontTraitMask : nuint {
		Italic = 1,
		Bold = 2,
		Unbold = 4,
		NonStandardCharacterSet = 8,
		Narrow = 0x10,
		Expanded = 0x20,
		Condensed = 0x40,
		SmallCaps = 0x80,
		Poster = 0x100,
		Compressed = 0x200,
		FixedPitch = 0x400,
		Unitalic = 0x1000000
	}
	
	[Flags]
	public enum NSPasteboardWritingOptions : nuint {
		WritingPromised = 1 << 9
	}

	public enum NSToolbarDisplayMode : nuint {
		Default, IconAndLabel, Icon, Label
	}

	public enum NSToolbarSizeMode : nuint {
		Default, Regular, Small
	}

	public enum NSAlertType {
		ErrorReturn = -2,
		OtherReturn,
		AlternateReturn,
		DefaultReturn
	}

	public enum NSPanelButtonType {
		Cancel, Ok
	}

	public enum NSTableViewColumnAutoresizingStyle : nuint {
		None = 0,
		Uniform,
		Sequential,
		ReverseSequential,
		LastColumnOnly,
		FirstColumnOnly
	}

	public enum NSTableViewSelectionHighlightStyle : nint {
		None = -1,
		Regular = 0,
		SourceList = 1
	}

	public enum NSTableViewDraggingDestinationFeedbackStyle : nint {
		None = -1,
		Regular = 0,
		SourceList = 1
	}

	public enum NSTableViewDropOperation : nuint {
		On,
		Above
	}

	[Flags]
	public enum NSTableColumnResizing : nuint {
		None = 0,
		Autoresizing = ( 1 << 0 ),
		UserResizingMask = ( 1 << 1 )
	} 

	[Flags]
	public enum NSTableViewGridStyle : nuint {
		None = 0,
		SolidVerticalLine   = 1 << 0,
		SolidHorizontalLine = 1 << 1,
		DashedHorizontalGridLine = 1 << 3
	}

	[Flags]
	public enum NSGradientDrawingOptions : nuint {
		None = 0,
		BeforeStartingLocation =   (1 << 0),
		AfterEndingLocation =    (1 << 1)
	}
	
	public enum NSImageAlignment : nuint {
		Center = 0,
		Top,
		TopLeft,
		TopRight,
		Left,
		Bottom,
		BottomLeft,
		BottomRight,
		Right
	}
	
	public enum NSImageFrameStyle : nuint {
		None = 0,
		Photo,
		GrayBezel,
		Groove,
		Button
	}
	
	public enum NSSpeechBoundary : nuint {
		Immediate =  0,
		hWord,
		Sentence
	}

	public enum NSSplitViewDividerStyle : nint {
		Thick = 1,
		Thin = 2,
		PaneSplitter = 3
	}
	
	public enum NSImageScaling : nuint {
		ProportionallyDown = 0,
		AxesIndependently,
		None,
		ProportionallyUpOrDown
	}
	
	public enum NSSegmentStyle : nint {
		Automatic = 0,
		Rounded = 1,
		TexturedRounded = 2,
		RoundRect = 3,
		TexturedSquare = 4,
		Capsule = 5,
		SmallSquare = 6
	}
	
	public enum NSSegmentSwitchTracking : nuint {
		SelectOne = 0,
		SelectAny = 1,
		Momentary = 2,
		MomentaryAccelerator = 3
	}
	
	public enum NSTickMarkPosition : nuint {
		Below,
		Above,
		Left = Above,
		Right = Below
	}
	
	public enum NSSliderType : nuint {
		Linear   = 0,
		Circular = 1
	}
	
	public enum NSTokenStyle : nuint {
		Default,
		PlainText,
		Rounded
	}

	[Flags]
	public enum NSWorkspaceLaunchOptions : nuint {
		Print = 2,
		InhibitingBackgroundOnly = 0x80,
		WithoutAddingToRecents = 0x100,
		WithoutActivation = 0x200,
		Async = 0x10000,
		AllowingClassicStartup = 0x20000,
		PreferringClassic = 0x40000,
		NewInstance = 0x80000,
		Hide = 0x100000,
		HideOthers = 0x200000,
		Default = Async | AllowingClassicStartup
	}

	[Flags]
	public enum NSWorkspaceIconCreationOptions : nuint {
		NSExcludeQuickDrawElements   = 1 << 1,
		NSExclude10_4Elements       = 1 << 2
	}

	public enum NSPathStyle : nint {
		NSPathStyleStandard,
		NSPathStyleNavigationBar,
		NSPathStylePopUp
	}

	public enum NSTabViewType : nuint {
		NSTopTabsBezelBorder,
		NSLeftTabsBezelBorder,
		NSBottomTabsBezelBorder,
		NSRightTabsBezelBorder,
		NSNoTabsBezelBorder,
		NSNoTabsLineBorder,
		NSNoTabsNoBorder,
	}

	public enum NSTabState : nuint {
		Selected, Background, Pressed
	}

	public enum NSLevelIndicatorStyle : nuint {
		Relevancy, ContinuousCapacity, DiscreteCapacity, RatingLevel
	}

	[Flags]
	public enum NSFontCollectionOptions : nuint {
		ApplicationOnlyMask = 1
	}

	public enum NSCollectionViewDropOperation : nint {
		On = 0, Before = 1
	}

	public enum NSDatePickerStyle : nuint {
		TextFieldAndStepper,
		ClockAndCalendar,
		TextField
	}

	public enum NSDatePickerMode : nuint {
		Single, Range
	}

	[Flags]
	public enum NSDatePickerElementFlags : nuint {
		HourMinute = 0xc,
		HourMinuteSecond = 0xe,
		TimeZone = 0x10,

		YearMonthDate = 0xc0,
		YearMonthDateDay = 0xe0,
		Era = 0x100
	}

	public enum NSOpenGLContextParameter : nint {
		[Obsolete] SwapRectangle = 200,
		[Obsolete] SwapRectangleEnable = 201,
		[Obsolete] RasterizationEnable = 221,
		[Obsolete] StateValidation = 301,
		[Obsolete] SurfaceSurfaceVolatile = 306,

		SwapInterval = 222,
		SurfaceOrder = 235,
		SurfaceOpacity = 236,

		[Lion] SurfaceBackingSize = 304,
		[Lion] ReclaimResources = 308,
		[Lion] CurrentRendererID = 309,
		[Lion] GpuVertexProcessing = 310,
		[Lion] GpuFragmentProcessing = 311,
		[Lion] HasDrawable = 314,
		[Lion] MpsSwapsInFlight = 315
	}
	
	public enum NSSurfaceOrder {
		AboveWindow = 1,
		BelowWindow = -1
	}

	public enum NSOpenGLPixelFormatAttribute {
		AllRenderers       =   1,
		DoubleBuffer       =   5,
		[Lion] TrippleBuffer = 3,
		Stereo             =   6,
		AuxBuffers         =   7,
		ColorSize          =   8,
		AlphaSize          =  11,
		DepthSize          =  12,
		StencilSize        =  13,
		AccumSize          =  14,
		MinimumPolicy      =  51,
		MaximumPolicy      =  52,
		OffScreen          =  53,
		FullScreen         =  54,
		SampleBuffers      =  55,
		Samples            =  56,
		AuxDepthStencil    =  57,
		ColorFloat         =  58,
		Multisample        =  59,
		Supersample        =  60,
		SampleAlpha        =  61,
		RendererID         =  70,
		SingleRenderer     =  71,
		NoRecovery         =  72,
		Accelerated        =  73,
		ClosestPolicy      =  74,
		BackingStore       =  76,
		Window             =  80,
		Compliant          =  83,
		ScreenMask         =  84,
		PixelBuffer        =  90,
		RemotePixelBuffer  =  91,
		AllowOfflineRenderers = 96,
		AcceleratedCompute =  97,

		// Specify the profile
		[Lion] OpenGLProfile = 99,
		VirtualScreenCount = 128,

		[Obsolete] Robust  =  75,
		[Obsolete] MPSafe  =  78,
		[Obsolete] MultiScreen =  81
	}

	public enum NSOpenGLProfile {
		VersionLegacy   = 0x1000, // Legacy
		Version3_2Core  = 0x3200  // 3.2 or better
	}
	
	public enum NSAlertButtonReturn {
		First = 1000,
		Second = 1001,
		Third = 1002,
	}

	public enum NSOpenGLGlobalOption {
		FormatCacheSize = 501,
		ClearFormatCache = 502,
		RetainRenderers = 503,
		[Lion] UseBuildCache = 506,
		[Obsolete] ResetLibrary = 504
	}

	public enum NSGLTextureTarget {
		T2D = 0x0de1,
		CubeMap = 0x8513,
		RectangleExt = 0x84F5,
	}

	public enum NSGLFormat {
		RGB = 0x1907,
		RGBA = 0x1908,
		DepthComponent = 0x1902,
	}
	
	public enum NSGLTextureCubeMap {
		None = 0,
		PositiveX = 0x8515,
		PositiveY = 0x8517,
		PositiveZ = 0x8519,
		NegativeX = 0x8516,
		NegativeY = 0x8517,
		NegativeZ = 0x851A
	}

	public enum NSGLColorBuffer {
		Front = 0x0404,
		Back = 0x0405,
		Aux0 = 0x0409
	}

	public enum NSProgressIndicatorThickness : nuint {
		Small = 10,
		Regular = 14,
		Aqua = 12,
		Large = 18
	}

	public enum NSProgressIndicatorStyle : nuint {
		Bar, Spinning
	}

	public enum NSPopUpArrowPosition : nuint {
		None,
		Center,
		Bottom
	}

	public static class NSFileTypeForHFSTypeCode {
		public static readonly string ComputerIcon = "root";
		public static readonly string DesktopIcon = "desk";
		public static readonly string FinderIcon = "FNDR";
	}
	
	// These constants specify the possible states of a drawer.
	public enum NSDrawerState : nuint {
		Closed = 0,
		Opening = 1,
		Open = 2,
		Closing = 3
	}

	public enum NSWindowLevel : nint {
		Normal = 0,
		Dock = 20,
		Floating = 3,
		MainMenu = 24, 
		ModalPanel = 8,
		PopUpMenu = 101,
		ScreenSaver = 1000,
		Status = 25,
		Submenu = 3,
		TornOffMenu = 3
	}
	
	public enum NSRuleEditorRowType : nuint {
		Simple = 0,
		Compound
	}
   
	public enum NSRuleEditorNestingMode : nuint {
		Single,
		List,
		Compound,
		Simple
	}

	public enum NSGlyphInscription : nuint {
		Base, Below, Above, Overstrike, OverBelow
	}

	public enum NSTypesetterBehavior : nint {
		Latest = -1,
		Original = 0,
		Specific_10_2_WithCompatibility = 1,
		Specific_10_2 = 2,
		Specific_10_3 = 3,
		Specific_10_4 = 4,
			
	}

	[Flags]
	public enum NSRemoteNotificationType : nuint {
		None = 0,
		Badge = 1,
		Sound = 2,
		Alert = 4
	}
	
	public enum NSScrollViewFindBarPosition : nint {
		AboveHorizontalRuler = 0,
		AboveContent,
		BelowContent
	}
	
	public enum NSScrollerStyle : nint {
   		Legacy = 0,
		Overlay
	}
	
	public enum  NSScrollElasticity : nint {
		Automatic = 0,
   		None,
		Allowed
	}
	
	public enum  NSScrollerKnobStyle : nint {
		Default  = 0,
		Dark     = 1,
		Light    = 2
	}

	[Flags]
	public enum NSEventPhase : nuint {
        None        = 0,
        Began       = 1 << 0,
        Stationary  = 1 << 1,
        Changed     = 1 << 2,
        Ended       = 1 << 3,
        Cancelled   = 1 << 4,
        MayBegin    = 1 << 5,
	}

	[Flags]
	public enum NSEventSwipeTrackingOptions : nuint {
		LockDirection = 1,
		ClampGestureAmount = 2
	}

	public enum NSEventGestureAxis : nint {
		None, Horizontal, Vertical
	}

	public enum NSLayoutRelation : nint {
		LessThanOrEqual = -1,
		Equal = 0,
		GreaterThanOrEqual = 1
	}

	public enum NSLayoutAttribute : nint {
		NoAttribute = 0,
		Left = 1,
		Right,
		Top,
		Bottom,
		Leading,
		Trailing,
		Width,
		Height,
		CenterX,
		CenterY,
		Baseline
	}

	public enum NSLayoutFormatOptions : nuint {
		None = 0,

		AlignAllLeft = (1 << (int)NSLayoutAttribute.Left),
		AlignAllRight = (1 << (int)NSLayoutAttribute.Right),
		AlignAllTop = (1 << (int)NSLayoutAttribute.Top),
		AlignAllBottom = (1 << (int)NSLayoutAttribute.Bottom),
		AlignAllLeading = (1 << (int)NSLayoutAttribute.Leading),
		AlignAllTrailing = (1 << (int)NSLayoutAttribute.Trailing),
		AlignAllCenterX = (1 << (int)NSLayoutAttribute.CenterX),
		AlignAllCenterY = (1 << (int)NSLayoutAttribute.CenterY),
		AlignAllBaseline = (1 << (int)NSLayoutAttribute.Baseline),
		
		AlignmentMask = 0xFFFF,
		
		/* choose only one of these three
		 */
		DirectionLeadingToTrailing = 0 << 16, // default
		DirectionLeftToRight = 1 << 16,
		DirectionRightToLeft = 2 << 16,
		
		DirectionMask = 0x3 << 16,
	}

	public enum NSLayoutConstraintOrientation : nint {
		Horizontal, Vertical
	}

	public enum NSLayoutPriority {
		Required = 1000,
		DefaultHigh = 750,
		DragThatCanResizeWindow = 510,
		WindowSizeStayPut = 500,
		DragThatCannotResizeWindow = 490,
		DefaultLow = 250,
		FittingSizeCompression = 50,
	}

	public enum NSPopoverAppearance : nint {
		Minimal, HUD
	}

	public enum NSPopoverBehavior : nint {
		ApplicationDefined, Transient, Semitransient
	}

	public enum NSTableViewRowSizeStyle : nint {
		Default = -1,
		Custom = 0,
		Small, Medium, Large
	}

	[Flags]
	public enum NSTableViewAnimation : nuint {
		None, Fade = 1, Gap = 2,
		SlideUp = 0x10, SlideDown = 0x20, SlideLeft = 0x30, SlideRight = 0x40
	}

	[Flags]
	public enum NSDraggingItemEnumerationOptions : nuint {
		Concurrent = 1 << 0,
		ClearNonenumeratedImages = 1 << 16
	}

	public enum NSDraggingFormation : nint {
		Default, None, Pile, List, Stack
	}

	public enum NSDraggingContext : nint {
		OutsideApplication, WithinApplication
	}

	public enum NSWindowAnimationBehavior : nint {
		Default = 0, None = 2, DocumentWindow, UtilityWindow, AlertPanel
	}

	[Lion]
	public enum NSTextFinderAction : nint {
		ShowFindInterface = 1,
		NextMatch = 2,
		PreviousMatch = 3,
		ReplaceAll = 4,
		Replace = 5,
		ReplaceAndFind = 6,
		SetSearchString = 7,
		ReplaceAllInSelection = 8,
		SelectAll = 9,
		SelectAllInSelection = 10,
		HideFindInterface = 11,
		ShowReplaceInterface = 12,
		HideReplaceInterface = 13
	}

	[Flags]
	public enum NSFontPanelMode {
		FaceMask = 1 << 0,
		SizeMask = 1 << 1,
		CollectionMask = 1 << 2,
		UnderlineEffectMask = 1<<8,
		StrikethroughEffectMask = 1<<9,
		TextColorEffectMask = 1<< 10,
		DocumentColorEffectMask = 1<<11,
		ShadowEffectMask = 1<<12,
		AllEffectsMask = 0XFFF00,
		StandardMask = 0xFFFF,
		AllModesMask = unchecked( (int)0xFFFFFFFF )
	}

	[Flags]
	public enum NSFontCollectionVisibility : nuint {
		Process = 1 << 0,
		User = 1 << 1,
		Computer = 1 << 2,
	}

	public enum NSSharingContentScope : nint {
		Item,
		Partial,
		Full
	}

	public enum NSSharingServiceName {
		PostOnFacebook,
		PostOnTwitter,
		PostOnSinaWeibo,
		ComposeEmail,
		ComposeMessage,
		SendViaAirDrop,
		AddToSafariReadingList,
		AddToIPhoto,
		AddToAperture,
		UseAsTwitterProfileImage,
		UseAsDesktopPicture,
		PostImageOnFlickr,
		PostVideoOnVimeo,
		PostVideoOnYouku,
		PostVideoOnTudou
	}

	[Flags]
	public enum NSTypesetterControlCharacterAction : nuint {
		ZeroAdvancement = 1 << 0,
		Whitespace = 1 << 1,
		HorizontalTab = 1 << 2,
		LineBreak = 1 << 3,
		ParagraphBreak = 1 << 4,
		ContainerBreak = 1 << 5,
	}
	
	public enum NSWindowTitleVisibility : long {
		Visible = 0,
		Hidden = 1,
	}
	
	[Flags]
	public enum NSWindowOcclusionState  : ulong {
		Visible = 1 << 1
	}

#region NSVisualEffectView
	// [NoMacCatalyst]
	[Native]
	public enum NSVisualEffectMaterial : long {
		// [Advice ("Use a specific material instead.")]
		AppearanceBased,
		// [Advice ("Use a semantic material instead.")]
		Light,
		// [Advice ("Use a semantic material instead.")]
		Dark,
		Titlebar,
		Selection,
		[Mac (10,11)]
		Menu,
		[Mac (10,11)]
		Popover,
		[Mac (10,11)]
		Sidebar,
		[Mac (10,11)]
		// [Advice ("Use a semantic material instead.")]
		MediumLight,
		[Mac (10,11)]
		// [Advice ("Use a semantic material instead.")]
		UltraDark,
		[Mac (10,14)]
		HeaderView = 10,
		[Mac (10,14)]
		Sheet = 11,
		[Mac (10,14)]
		WindowBackground = 12,
		[Mac (10,14)]
		HudWindow = 13,
		[Mac (10,14)]
		FullScreenUI = 15,
		[Mac (10,14)]
		ToolTip = 17,
		[Mac (10,14)]
		ContentBackground = 18,
		[Mac (10,14)]
		UnderWindowBackground = 21,
		[Mac (10,14)]
		UnderPageBackground = 22,
	}

	// [NoMacCatalyst]
	[Native]
	public enum NSVisualEffectBlendingMode : long {
		BehindWindow,
		WithinWindow
	}

	// [NoMacCatalyst]
	[Native]
	public enum NSVisualEffectState : long {
		FollowsWindowActiveState,
		Active,
		Inactive
	}
#endregion



	[Mac (10, 12)]
	public enum NSDisplayGamut : long
	{
		Srgb = 1,
		P3,
	}
	
	public enum NSStatusItemLength {
		Variable = -1,
		Square = -2
	}
	
	[Mac (10,12)]
	//[Native]
	public enum NSWindowTabbingMode : nint
	{
		Automatic,
		Preferred,
		Disallowed
	}

	[Mac (10,12)]
	//[Native]
	public enum NSWindowUserTabbingPreference : nint
	{
		Manual,
		Always,
		InFullScreen
	}

    [Mac(10, 13)]
    //[Native]
    public enum NSColorType : long
    {
        ComponentBased,
        Pattern,
        Catalog,
    }

    [Mac(10, 14, onlyOn64: true)]
    //[Native]
    public enum NSColorSystemEffect : long
    {
        None,
        Pressed,
        DeepPressed,
        Disabled,
        Rollover,
    }

	// [NoMacCatalyst]
	[Mac (11, 0)]
	// [Native]
	public enum NSTableViewStyle : long {
		Automatic,
		FullWidth,
		Inset,
		SourceList,
		Plain,
	}

	[Mac (11,0)]
	// [Native]
	public enum NSTitlebarSeparatorStyle : long
	{
		Automatic,
		None,
		Line,
		Shadow,
	}

	[Mac (11,0)]
	// [Native]
	public enum NSWindowToolbarStyle : long
	{
		Automatic,
		Expanded,
		Preference,
		Unified,
		UnifiedCompact,
	}

    [Mac(10, 13)]
    //[Native]
    public enum NSSegmentDistribution : long
    {
        Fit = 0,
        Fill,
        FillEqually,
        FillProportionally,
    }
}
