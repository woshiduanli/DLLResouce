using System;
using System.Runtime.InteropServices;
using System.Text;

static class Win32 {

	public const int MAX_PATH = 260;

	public enum InternalCursor {
		Arrow = 32512,
		Ibeam,
		Wait,
		Cross,
		UpArrow,
		SizeNWSE = 32642,
		SizeNESW,
		SizeWE,
		SizeNS,
		SizeAll,
		SizeNo = 32648,
		Hand,
		AppStarting,
		Help
	};

	private const string DLL_KERNEL32 = "Kernel32";
	private const string DLL_USER32 = "User32";
	private const string DLL_SHELL32 = "Shell32";

	[DllImport(DLL_USER32, CharSet = CharSet.Unicode)]
	public static extern IntPtr LoadCursorW(IntPtr hInstance, string cursorName);

	[DllImport(DLL_USER32)]
	public static extern IntPtr LoadCursorW(IntPtr hInstance, int cursor);

	public static IntPtr LoadCursorInternal(InternalCursor ic) {
		return LoadCursorW(IntPtr.Zero, (int)ic);
	}

	[DllImport(DLL_USER32)]
	public static extern IntPtr SetCursor(IntPtr cursor);

	public static IntPtr SetCursor(InternalCursor ic) {
		return SetCursor(LoadCursorInternal(ic));
	}

	[DllImport(DLL_USER32)]
	public extern static int SetCursorPos(int x, int y);

	public struct Point {
		public int x;
		public int y;
	}

	[DllImport(DLL_USER32)]
	public extern static int GetCursorPos(out Point point);

	#region 多字节字符转Unicode

	[DllImport(DLL_KERNEL32)]
	public static extern int MultiByteToWideChar(
		uint codePage, uint flags,
		IntPtr multiBytes, int cbMultiBytes,
		IntPtr wideChars, int cchWideChars);

	public static string MultiByteToWideChar(
		uint codePage, uint flags,
		IntPtr multiBytes, int cbMultiBytes
	) {
		int chars = MultiByteToWideChar(codePage, flags, multiBytes, cbMultiBytes, IntPtr.Zero, 0);
		if (chars <= 0) { return string.Empty; }
		IntPtr dest = Marshal.AllocHGlobal(chars * sizeof(char));
		try {
			MultiByteToWideChar(codePage, flags, multiBytes, cbMultiBytes, dest, chars);
			return Marshal.PtrToStringUni(dest, chars);
		} finally {
			Marshal.FreeHGlobal(dest);
		}
	}

	public static string MultiByteToWideChar(uint codePage, uint flags, byte[] multiBytes) {
		IntPtr sour = Marshal.AllocHGlobal(multiBytes.Length);
		try {
			Marshal.Copy(multiBytes, 0, sour, multiBytes.Length);
			return MultiByteToWideChar(codePage, flags, sour, multiBytes.Length);
		} finally {
			Marshal.FreeHGlobal(sour);
		}
	}

	public static string MultiByteToWideChar_GB2312(IntPtr multiBytes, int cbMultiBytes) {
		return MultiByteToWideChar(936, 0, multiBytes, cbMultiBytes);
	}

	public static string MultiByteToWideChar_GB2312(byte[] multiBytes) {
		return MultiByteToWideChar(936, 0, multiBytes);
	}

	#endregion


	#region Unicode转多字节字符

	[DllImport(DLL_KERNEL32, CharSet = CharSet.Unicode)]
	public static extern int WideCharToMultiByte(uint codePage, uint flags,
		string wideChar, int cchWideChar,
		IntPtr multiByteStr, int cbMultiByte,
		IntPtr p1, IntPtr p2);

	//public static string MultiByteToWideChar(IntPtr multiByte, int bytes) {
	//    int chars = MultiByteToWideChar(0, 0, multiByte, bytes, null, 0);
	//    if (chars <= 0) {
	//        return string.Empty;
	//    }
	//    byte[] wc = new byte[chars * 2];
	//    MultiByteToWideChar(0, 0, multiByte, bytes, wc, wc.Length);
	//    return Encoding.Unicode.GetString(wc);
	//}




	#endregion

	[DllImport( DLL_USER32, SetLastError = true )]
    public static extern short GetKeyState( int nVirtKey );
#if false
	public static bool IsKeyHolding(VirtualKey vk) {
		return GetKeyState((int)vk) < 0;
	}
#endif

	[DllImport(DLL_KERNEL32)]
	public static extern uint GetCurrentThreadId();

	[DllImport(DLL_KERNEL32, CharSet=CharSet.Unicode)]
	public static extern string GetCommandLineW();

	

	[DllImport(DLL_USER32)]
	public static extern int OpenClipboard(int wnd);

	[DllImport(DLL_USER32)]
	public static extern int EmptyClipboard();

	[DllImport(DLL_USER32)]
	public static extern int CloseClipboard();

	[DllImport(DLL_USER32)]
	public static extern int GetClipboardData(uint format);

    [DllImport( DLL_USER32 )]
    public static extern int SetClipboardData( uint format, int hMem );

	[DllImport(DLL_USER32)]
	public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
	
	public delegate int WNDENUMPROC(IntPtr hwnd, IntPtr param);

	[DllImport(DLL_USER32)]
	public static extern int EnumWindows(WNDENUMPROC lpEnumFunc, IntPtr lParam);

	[DllImport(DLL_USER32)]
	public static extern int GetWindowRect(IntPtr hWnd, out RECT rc);

	[Flags]
	public enum SetWindowPosFlags {
		SWP_NOSIZE = 0x0001,
		SWP_NOMOVE = 0x0002,
		SWP_NOZORDER = 0x0004,
		SWP_NOREDRAW = 0x0008
	}
	
	[DllImport(DLL_USER32)]
	public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

	public enum ClipboardDataFormat {
		CF_TEXT = 1,
		CF_BITMAP,
		CF_METAFILEPICT,
		CF_SYLK,
		CF_DIF,
		CF_TIFF,
		CF_OEMTEXT,
		CF_DIB,
		CF_PALETTE,
		CF_PENDATA,
		CF_RIFF,
		CF_WAVE,
		CF_UNICODETEXT,
		CF_ENHMETAFILE
	};

	public const uint GMEM_FIXED = 0x0000;
	public const uint GMEM_MOVEABLE = 0x0002;

	[DllImport(DLL_KERNEL32)]
	public static extern int GlobalAlloc(uint flags, int bytes);

	[DllImport(DLL_KERNEL32)]
	public static extern int GlobalFree(int hMem);

	[DllImport(DLL_KERNEL32)]
	public static extern IntPtr GlobalLock(int hMem);

	[DllImport(DLL_KERNEL32)]
	public static extern int GlobalUnlock(int hMem);

	[DllImport(DLL_KERNEL32)]
	public static extern void RtlMoveMemory(IntPtr dest, IntPtr sour, uint size);

	[DllImport(DLL_KERNEL32, CharSet = CharSet.Unicode, EntryPoint = "RtlMoveMemory")]
	public static extern void CopyString(IntPtr dest, string sour, uint size);

	[DllImport(DLL_KERNEL32)]
	public static extern uint GetCurrentProcessId();

	[DllImport(DLL_KERNEL32)]
	public static extern void ExitProcess(uint exit_code);

	[DllImport(DLL_KERNEL32)]
	public static extern void Sleep(uint milliseconds);

	public enum VirtualKey {
		VK_LBUTTON = 0x01,
		VK_RBUTTON = 0x02,
		VK_CANCEL = 0x03,
		VK_MBUTTON = 0x04,    /* NOT contiguous with L & RBUTTON */


		VK_XBUTTON1 = 0x05,    /* NOT contiguous with L & RBUTTON */
		VK_XBUTTON2 = 0x06,    /* NOT contiguous with L & RBUTTON */

		/*
		 * 0x07 : unassigned
		 */

		VK_BACK = 0x08,
		VK_TAB = 0x09,

		/*
		 * 0x0A - 0x0B : reserved
		 */

		VK_CLEAR = 0x0C,
		VK_RETURN = 0x0D,

		VK_SHIFT = 0x10,
		VK_CONTROL = 0x11,
		VK_MENU = 0x12,
		VK_PAUSE = 0x13,
		VK_CAPITAL = 0x14,

		VK_KANA = 0x15,
		VK_HANGEUL = 0x15,  /* old name - should be here for compatibility */
		VK_HANGUL = 0x15,
		VK_JUNJA = 0x17,
		VK_FINAL = 0x18,
		VK_HANJA = 0x19,
		VK_KANJI = 0x19,

		VK_ESCAPE = 0x1B,

		VK_CONVERT = 0x1C,
		VK_NONCONVERT = 0x1D,
		VK_ACCEPT = 0x1E,
		VK_MODECHANGE = 0x1F,

		VK_SPACE = 0x20,
		VK_PRIOR = 0x21,
		VK_NEXT = 0x22,
		VK_END = 0x23,
		VK_HOME = 0x24,
		VK_LEFT = 0x25,
		VK_UP = 0x26,
		VK_RIGHT = 0x27,
		VK_DOWN = 0x28,
		VK_SELECT = 0x29,
		VK_PRINT = 0x2A,
		VK_EXECUTE = 0x2B,
		VK_SNAPSHOT = 0x2C,
		VK_INSERT = 0x2D,
		VK_DELETE = 0x2E,
		VK_HELP = 0x2F,

		/*
		 * VK_0 - VK_9 are the same as ASCII '0' - '9' (0x30 - 0x39)
		 * 0x40 : unassigned
		 * VK_A - VK_Z are the same as ASCII 'A' - 'Z' (0x41 - 0x5A)
		 */

		VK_LWIN = 0x5B,
		VK_RWIN = 0x5C,
		VK_APPS = 0x5D,

		/*
		 * 0x5E : reserved
		 */

		VK_SLEEP = 0x5F,

		VK_NUMPAD0 = 0x60,
		VK_NUMPAD1 = 0x61,
		VK_NUMPAD2 = 0x62,
		VK_NUMPAD3 = 0x63,
		VK_NUMPAD4 = 0x64,
		VK_NUMPAD5 = 0x65,
		VK_NUMPAD6 = 0x66,
		VK_NUMPAD7 = 0x67,
		VK_NUMPAD8 = 0x68,
		VK_NUMPAD9 = 0x69,
		VK_MULTIPLY = 0x6A,
		VK_ADD = 0x6B,
		VK_SEPARATOR = 0x6C,
		VK_SUBTRACT = 0x6D,
		VK_DECIMAL = 0x6E,
		VK_DIVIDE = 0x6F,
		VK_F1 = 0x70,
		VK_F2 = 0x71,
		VK_F3 = 0x72,
		VK_F4 = 0x73,
		VK_F5 = 0x74,
		VK_F6 = 0x75,
		VK_F7 = 0x76,
		VK_F8 = 0x77,
		VK_F9 = 0x78,
		VK_F10 = 0x79,
		VK_F11 = 0x7A,
		VK_F12 = 0x7B,
		VK_F13 = 0x7C,
		VK_F14 = 0x7D,
		VK_F15 = 0x7E,
		VK_F16 = 0x7F,
		VK_F17 = 0x80,
		VK_F18 = 0x81,
		VK_F19 = 0x82,
		VK_F20 = 0x83,
		VK_F21 = 0x84,
		VK_F22 = 0x85,
		VK_F23 = 0x86,
		VK_F24 = 0x87,

		/*
		 * 0x88 - 0x8F : unassigned
		 */

		VK_NUMLOCK = 0x90,
		VK_SCROLL = 0x91,

		/*
		 * NEC PC-9800 kbd definitions
		 */
		VK_OEM_NEC_EQUAL = 0x92,   // '=' key on numpad

		/*
		 * Fujitsu/OASYS kbd definitions
		 */
		VK_OEM_FJ_JISHO = 0x92,   // 'Dictionary' key
		VK_OEM_FJ_MASSHOU = 0x93,   // 'Unregister word' key
		VK_OEM_FJ_TOUROKU = 0x94,   // 'Register word' key
		VK_OEM_FJ_LOYA = 0x95,   // 'Left OYAYUBI' key
		VK_OEM_FJ_ROYA = 0x96,   // 'Right OYAYUBI' key

		/*
		 * 0x97 - 0x9F : unassigned
		 */

		/*
		 * VK_L* & VK_R* - left and right Alt, Ctrl and Shift virtual keys.
		 * Used only as parameters to GetAsyncKeyState() and GetKeyState().
		 * No other API or message will distinguish left and right keys in this way.
		 */
		VK_LSHIFT = 0xA0,
		VK_RSHIFT = 0xA1,
		VK_LCONTROL = 0xA2,
		VK_RCONTROL = 0xA3,
		VK_LMENU = 0xA4,
		VK_RMENU = 0xA5,


		VK_BROWSER_BACK = 0xA6,
		VK_BROWSER_FORWARD = 0xA7,
		VK_BROWSER_REFRESH = 0xA8,
		VK_BROWSER_STOP = 0xA9,
		VK_BROWSER_SEARCH = 0xAA,
		VK_BROWSER_FAVORITES = 0xAB,
		VK_BROWSER_HOME = 0xAC,

		VK_VOLUME_MUTE = 0xAD,
		VK_VOLUME_DOWN = 0xAE,
		VK_VOLUME_UP = 0xAF,
		VK_MEDIA_NEXT_TRACK = 0xB0,
		VK_MEDIA_PREV_TRACK = 0xB1,
		VK_MEDIA_STOP = 0xB2,
		VK_MEDIA_PLAY_PAUSE = 0xB3,
		VK_LAUNCH_MAIL = 0xB4,
		VK_LAUNCH_MEDIA_SELECT = 0xB5,
		VK_LAUNCH_APP1 = 0xB6,
		VK_LAUNCH_APP2 = 0xB7,


		/*
		 * 0xB8 - 0xB9 : reserved
		 */

		VK_OEM_1 = 0xBA,   // ';:' for US
		VK_OEM_PLUS = 0xBB,   // '+' any country
		VK_OEM_COMMA = 0xBC,   // ',' any country
		VK_OEM_MINUS = 0xBD,   // '-' any country
		VK_OEM_PERIOD = 0xBE,   // '.' any country
		VK_OEM_2 = 0xBF,   // '/?' for US
		VK_OEM_3 = 0xC0,   // '`~' for US

		/*
		 * 0xC1 - 0xD7 : reserved
		 */

		/*
		 * 0xD8 - 0xDA : unassigned
		 */

		VK_OEM_4 = 0xDB,  //  '[{' for US
		VK_OEM_5 = 0xDC,  //  '\|' for US
		VK_OEM_6 = 0xDD,  //  ']}' for US
		VK_OEM_7 = 0xDE,  //  ''"' for US
		VK_OEM_8 = 0xDF,

		/*
		 * 0xE0 : reserved
		 */

		/*
		 * Various extended or enhanced keyboards
		 */
		VK_OEM_AX = 0xE1,  //  'AX' key on Japanese AX kbd
		VK_OEM_102 = 0xE2,  //  "<>" or "\|" on RT 102-key kbd.
		VK_ICO_HELP = 0xE3,  //  Help key on ICO
		VK_ICO_00 = 0xE4,  //  00 key on ICO


		VK_PROCESSKEY = 0xE5,


		VK_ICO_CLEAR = 0xE6,



		VK_PACKET = 0xE7,


		/*
		 * 0xE8 : unassigned
		 */

		/*
		 * Nokia/Ericsson definitions
		 */
		VK_OEM_RESET = 0xE9,
		VK_OEM_JUMP = 0xEA,
		VK_OEM_PA1 = 0xEB,
		VK_OEM_PA2 = 0xEC,
		VK_OEM_PA3 = 0xED,
		VK_OEM_WSCTRL = 0xEE,
		VK_OEM_CUSEL = 0xEF,
		VK_OEM_ATTN = 0xF0,
		VK_OEM_FINISH = 0xF1,
		VK_OEM_COPY = 0xF2,
		VK_OEM_AUTO = 0xF3,
		VK_OEM_ENLW = 0xF4,
		VK_OEM_BACKTAB = 0xF5,

		VK_ATTN = 0xF6,
		VK_CRSEL = 0xF7,
		VK_EXSEL = 0xF8,
		VK_EREOF = 0xF9,
		VK_PLAY = 0xFA,
		VK_ZOOM = 0xFB,
		VK_NONAME = 0xFC,
		VK_PA1 = 0xFD,
		VK_OEM_CLEAR = 0xFE,
	};

	[DllImport(DLL_SHELL32, CharSet=CharSet.Unicode)]
	public extern static IntPtr ShellExecuteW(IntPtr wnd, string operation, string file, string parameters, string directory, int show);

	[DllImport(DLL_SHELL32, CharSet = CharSet.Unicode)]
	private extern static int SHGetFolderPath(
		System.IntPtr wnd,
		int folder,
		int token,
		uint flags, System.IntPtr path);

	public static string SHGetFolderPath(CSIDL folder) {
		string result = null;
		System.IntPtr p = Marshal.AllocHGlobal(2 * (MAX_PATH + 1));
		try {
			if (0 == Win32.SHGetFolderPath(System.IntPtr.Zero, (int)folder, 0, 0, p)) {
				result = Marshal.PtrToStringUni(p);
			}
		} finally {
			Marshal.FreeHGlobal(p);
		}
		return result;
	}

	public enum CSIDL : int {
		CSIDL_DESKTOP = 0x0000,        // <desktop>
		CSIDL_INTERNET = 0x0001,        // Internet Explorer (icon on desktop)
		CSIDL_PROGRAMS = 0x0002,        // Start Menu\Programs
		CSIDL_CONTROLS = 0x0003,        // My Computer\Control Panel
		CSIDL_PRINTERS = 0x0004,        // My Computer\Printers
		CSIDL_PERSONAL = 0x0005,        // My Documents
		CSIDL_FAVORITES = 0x0006,        // <user name>\Favorites
		CSIDL_STARTUP = 0x0007,        // Start Menu\Programs\Startup
		CSIDL_RECENT = 0x0008,        // <user name>\Recent
		CSIDL_SENDTO = 0x0009,        // <user name>\SendTo
		CSIDL_BITBUCKET = 0x000a,        // <desktop>\Recycle Bin
		CSIDL_STARTMENU = 0x000b,        // <user name>\Start Menu
		CSIDL_MYDOCUMENTS = CSIDL_PERSONAL, //  Personal was just a silly name for My Documents
		CSIDL_MYMUSIC = 0x000d,        // "My Music" folder
		CSIDL_MYVIDEO = 0x000e,        // "My Videos" folder
		CSIDL_DESKTOPDIRECTORY = 0x0010,        // <user name>\Desktop
		CSIDL_DRIVES = 0x0011,        // My Computer
		CSIDL_NETWORK = 0x0012,        // Network Neighborhood (My Network Places)
		CSIDL_NETHOOD = 0x0013,        // <user name>\nethood
		CSIDL_FONTS = 0x0014,        // windows\fonts
		CSIDL_TEMPLATES = 0x0015,
		CSIDL_COMMON_STARTMENU = 0x0016,        // All Users\Start Menu
		CSIDL_COMMON_PROGRAMS = 0X0017,        // All Users\Start Menu\Programs
		CSIDL_COMMON_STARTUP = 0x0018,        // All Users\Startup
		CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019,        // All Users\Desktop
		CSIDL_APPDATA = 0x001a,        // <user name>\Application Data
		CSIDL_PRINTHOOD = 0x001b,        // <user name>\PrintHood

		CSIDL_LOCAL_APPDATA = 0x001c,        // <user name>\Local Settings\Applicaiton Data (non roaming)


		CSIDL_ALTSTARTUP = 0x001d,        // non localized startup
		CSIDL_COMMON_ALTSTARTUP = 0x001e,        // non localized common startup
		CSIDL_COMMON_FAVORITES = 0x001f,

		CSIDL_INTERNET_CACHE = 0x0020,
		CSIDL_COOKIES = 0x0021,
		CSIDL_HISTORY = 0x0022,
		CSIDL_COMMON_APPDATA = 0x0023,        // All Users\Application Data
		CSIDL_WINDOWS = 0x0024,        // GetWindowsDirectory()
		CSIDL_SYSTEM = 0x0025,        // GetSystemDirectory()
		CSIDL_PROGRAM_FILES = 0x0026,        // C:\Program Files
		CSIDL_MYPICTURES = 0x0027,        // C:\Program Files\My Pictures

		CSIDL_PROFILE = 0x0028,        // USERPROFILE
		CSIDL_SYSTEMX86 = 0x0029,        // x86 system directory on RISC
		CSIDL_PROGRAM_FILESX86 = 0x002a,        // x86 C:\Program Files on RISC

		CSIDL_PROGRAM_FILES_COMMON = 0x002b,        // C:\Program Files\Common


		CSIDL_PROGRAM_FILES_COMMONX86 = 0x002c,        // x86 Program Files\Common on RISC
		CSIDL_COMMON_TEMPLATES = 0x002d,        // All Users\Templates

		CSIDL_COMMON_DOCUMENTS = 0x002e,        // All Users\Documents
		CSIDL_COMMON_ADMINTOOLS = 0x002f,        // All Users\Start Menu\Programs\Administrative Tools
		CSIDL_ADMINTOOLS = 0x0030,        // <user name>\Start Menu\Programs\Administrative Tools


		CSIDL_CONNECTIONS = 0x0031,        // Network and Dial-up Connections
		CSIDL_COMMON_MUSIC = 0x0035,        // All Users\My Music
		CSIDL_COMMON_PICTURES = 0x0036,        // All Users\My Pictures
		CSIDL_COMMON_VIDEO = 0x0037,        // All Users\My Video
		CSIDL_RESOURCES = 0x0038,        // Resource Direcotry


		CSIDL_RESOURCES_LOCALIZED = 0x0039,        // Localized Resource Direcotry


		CSIDL_COMMON_OEM_LINKS = 0x003a,        // Links to All Users OEM specific apps
		CSIDL_CDBURN_AREA = 0x003b,        // USERPROFILE\Local Settings\Application Data\Microsoft\CD Burning
		// unused                               0x003c
		CSIDL_COMPUTERSNEARME = 0x003d,        // Computers Near Me (computered from Workgroup membership)


		CSIDL_FLAG_CREATE = 0x8000,        // combine with CSIDL_ value to force folder creation in SHGetFolderPath()


		CSIDL_FLAG_DONT_VERIFY = 0x4000,        // combine with CSIDL_ value to return an unverified folder path
		CSIDL_FLAG_DONT_UNEXPAND = 0x2000,        // combine with CSIDL_ value to avoid unexpanding environment variables

		CSIDL_FLAG_NO_ALIAS = 0x1000,        // combine with CSIDL_ value to insure non-alias versions of the pidl
		CSIDL_FLAG_PER_USER_INIT = 0x0800,        // combine with CSIDL_ value to indicate per-user init (eg. upgrade)

		CSIDL_FLAG_MASK = 0xFF00,        // mask for all possible flag values
	}

	[DllImport(DLL_KERNEL32, CharSet=CharSet.Unicode)]
	private static extern uint GetModuleFileNameW(int hModule, IntPtr filename, uint bufSize);

	public static string GetModuleFileName(int hModule) {
		IntPtr buf = Marshal.AllocHGlobal(1024);
		try {
			if (GetModuleFileNameW(hModule, buf, 512) > 0) {
				return Marshal.PtrToStringUni(buf);
			} else {
				return string.Empty;
			}
		} finally {
			Marshal.FreeHGlobal(buf);
		}
	}

	////////////////////////////////////////////////

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct RECT {
		public int left;
        public int top;
        public int right;
        public int bottom;
	};

    [DllImport( "dph.ldr" )]
    public static extern int __initialize();

#if false

	[DllImport(DLL_KERNEL32, CharSet=CharSet.Unicode)]
	public static extern IntPtr GetModuleHandleW(string name);

	[DllImport("comdlg32", CharSet = CharSet.Unicode)]
	private static extern int GetOpenFileNameW(IntPtr options);

	private struct OPENFILENAME {
		public int lStructSize;
		public IntPtr hwndOwner;
		public IntPtr hInstance;
		public IntPtr lpstrFilter;
		public IntPtr lpstrCustomFilter;
		public int nMaxCustFilter;
		public int nFilterIndex;
		public IntPtr lpstrFile;
		public int nMaxFile;
		public IntPtr lpstrFileTitle;
		public int nMaxFileTitle;
		public IntPtr lpstrInitialDir;
		public IntPtr lpstrTitle;
		public int Flags;
		public short nFileOffset;
		public short nFileExtension;
		public IntPtr lpstrDefExt;
		public int lCustData;
		public IntPtr lpfnHook;
		public IntPtr lpTemplateName;
		public IntPtr pvReserved;
		public int dwReserved;
		public uint FlagsEx;
	}

	public static string GetOpenFileName() {
		string result = null;
		IntPtr buf = Marshal.AllocHGlobal(512);
		try {
			OPENFILENAME ofn = new OPENFILENAME();
			ofn.lStructSize = Marshal.SizeOf(typeof(OPENFILENAME));
			ofn.hInstance = GetModuleHandleW(null);
			ofn.lpstrFile = buf;
			ofn.nMaxFile = 512;
			//
			IntPtr ofPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(OPENFILENAME)));
			try {
				Marshal.StructureToPtr(ofn, ofPtr, false);
				if (0 != GetOpenFileNameW(ofPtr)) {
					result = Marshal.PtrToStringUni(buf);
				}
			} finally {
				Marshal.FreeHGlobal(ofPtr);
			}
		} finally {
			Marshal.FreeHGlobal(buf);
		}
		return result;
	}
#endif

    #region MONO DLL
    [DllImport( "mono", EntryPoint = "mono_gc_collect" )]
    public static extern void MonoGC( int g );

    [DllImport( "mono", EntryPoint = "mono_gc_get_heap_size" )]
    public static extern int MonoGetHeapSize();

    [DllImport( "mono", EntryPoint = "mono_gc_get_used_size" )]
    public static extern int MonoGetUsedSize();
    #endregion
}


