using System;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable once CommentTypo
// ReSharper disable MemberCanBeinternal.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable ClassNeverInstantiated.Global
#pragma warning disable 649
#pragma warning disable 414

namespace Chromely.Dialogs.Windows
{
    internal static class WindowsInterop
    {
        // MessageBox() Flags
        internal const uint MB_OK = (uint) 0x00000000L;
        internal const uint MB_OKCANCEL = (uint) 0x00000001L;
        internal const uint MB_ABORTRETRYIGNORE = (uint) 0x00000002L;
        internal const uint MB_YESNOCANCEL = (uint) 0x00000003L;
        internal const uint MB_YESNO = (uint) 0x00000004L;
        internal const uint MB_RETRYCANCEL = (uint) 0x00000005L;
        internal const uint MB_CANCELTRYCONTINUE = (uint) 0x00000006L;

        internal const uint MB_ICONHAND = (uint) 0x00000010L;
        internal const uint MB_ICONQUESTION = (uint) 0x00000020L;
        internal const uint MB_ICONEXCLAMATION = (uint) 0x00000030L;
        internal const uint MB_ICONASTERISK = (uint) 0x00000040L;

        internal const uint MB_USERICON = (uint) 0x00000080L;
        internal const uint MB_ICONWARNING = MB_ICONEXCLAMATION;
        internal const uint MB_ICONERROR = MB_ICONHAND;

        internal const uint MB_ICONINFORMATION = MB_ICONASTERISK;
        internal const uint MB_ICONSTOP = MB_ICONHAND;

        internal const uint MB_DEFBUTTON1 = (uint) 0x00000000L;
        internal const uint MB_DEFBUTTON2 = (uint) 0x00000100L;
        internal const uint MB_DEFBUTTON3 = (uint) 0x00000200L;
        internal const uint MB_DEFBUTTON4 = (uint) 0x00000300L;

        internal const uint MB_APPLMODAL = (uint) 0x00000000L;
        internal const uint MB_SYSTEMMODAL = (uint) 0x00001000L;
        internal const uint MB_TASKMODAL = (uint) 0x00002000L;
        internal const uint MB_HELP = (uint) 0x00004000L; // Help Button

        internal const uint MB_NOFOCUS = (uint) 0x00008000L;
        internal const uint MB_SETFOREGROUND = (uint) 0x00010000L;
        internal const uint MB_DEFAULT_DESKTOP_ONLY = (uint) 0x00020000L;

        internal const uint MB_TOPMOST = (uint) 0x00040000L;
        internal const uint MB_RIGHT = (uint) 0x00080000L;
        internal const uint MB_RTLREADING = (uint) 0x00100000L;

        internal const uint MB_SERVICE_NOTIFICATION = (uint) 0x00200000L;

        
        // Dialog Box Command IDs
        internal const int IDOK = 1;
        internal const int IDCANCEL = 2;
        internal const int IDABORT = 3;
        internal const int IDRETRY = 4;
        internal const int IDIGNORE = 5;
        internal const int IDYES = 6;
        internal const int IDNO = 7;
        internal const int IDCLOSE = 8;
        internal const int IDHELP = 9;
        internal const int IDTRYAGAIN = 10;
        internal const int IDCONTINUE = 11;
        internal const int IDTIMEOUT = 32000;

        
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

        
        internal delegate int BrowseCallbackProc(IntPtr hwnd, int uMsg, IntPtr lParam, IntPtr lpData);

        [StructLayout(LayoutKind.Sequential)]
        internal struct BROWSEINFO 
        {
            public IntPtr hwndOwner;
            public IntPtr pidlRoot;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pszDisplayName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpszTitle;
            public uint ulFlags;
            public BrowseCallbackProc lpfn;
            public IntPtr lParam;
            public int iImage;
        }

          // Constants for sending and receiving messages in BrowseCallBackProc
        public const int WM_USER = 0x400;
        public const int BFFM_INITIALIZED = 1;
        public const int BFFM_SELCHANGED = 2;
        public const int BFFM_VALIDATEFAILEDA = 3;
        public const int BFFM_VALIDATEFAILEDW = 4;
        public const int BFFM_IUNKNOWN = 5; // provides IUnknown to client. lParam: IUnknown*
        public const int BFFM_SETSTATUSTEXTA = WM_USER + 100;
        public const int BFFM_ENABLEOK = WM_USER + 101;
        public const int BFFM_SETSELECTIONA = WM_USER + 102;
        public const int BFFM_SETSELECTIONW = WM_USER + 103;
        public const int BFFM_SETSTATUSTEXTW = WM_USER + 104;
        public const int BFFM_SETOKTEXT = WM_USER + 105; // Unicode only
        public const int BFFM_SETEXPANDED = WM_USER + 106; // Unicode only

        // Browsing for directory.
        internal const uint BIF_RETURNONLYFSDIRS   = 0x0001;  // For finding a folder to start document searching
        internal const uint BIF_DONTGOBELOWDOMAIN  = 0x0002;  // For starting the Find Computer
        internal const uint BIF_STATUSTEXT     = 0x0004;  // Top of the dialog has 2 lines of text for BROWSEINFO.lpszTitle and one line if
        // this flag is set.  Passing the message BFFM_SETSTATUSTEXTA to the hwnd can set the
        // rest of the text.  This is not used with BIF_USENEWUI and BROWSEINFO.lpszTitle gets
        // all three lines of text.
        internal const uint BIF_RETURNFSANCESTORS  = 0x0008;
        internal const uint BIF_EDITBOX        = 0x0010;   // Add an editbox to the dialog
        internal const uint BIF_VALIDATE       = 0x0020;   // insist on valid result (or CANCEL)

        internal const uint BIF_NEWDIALOGSTYLE     = 0x0040;   // Use the new dialog layout with the ability to resize
        // Caller needs to call OleInitialize() before using this API
        internal const uint BIF_USENEWUI  = 0x0040 + 0x0010; //(BIF_NEWDIALOGSTYLE | BIF_EDITBOX);

        internal const uint BIF_BROWSEINCLUDEURLS  = 0x0080;   // Allow URLs to be displayed or entered. (Requires BIF_USENEWUI)
        internal const uint BIF_UAHINT         = 0x0100;   // Add a UA hint to the dialog, in place of the edit box. May not be combined with BIF_EDITBOX
        internal const uint BIF_NONEWFOLDERBUTTON  = 0x0200;   // Do not add the "New Folder" button to the dialog.  Only applicable with BIF_NEWDIALOGSTYLE.
        internal const uint BIF_NOTRANSLATETARGETS = 0x0400;  // don't traverse target as shortcut

        internal const uint BIF_BROWSEFORCOMPUTER  = 0x1000;  // Browsing for Computers.
        internal const uint BIF_BROWSEFORPRINTER   = 0x2000;// Browsing for Printers
        internal const uint BIF_BROWSEINCLUDEFILES = 0x4000; // Browsing for Everything
        internal const uint BIF_SHAREABLE      = 0x8000;  // sharable resources displayed (remote shares, requires BIF_USENEWUI)

        [DllImport("shell32.dll")]
        internal static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lpbi);

        // Note that the BROWSEINFO object's pszDisplayName only gives you the name of the folder.
        // To get the actual path, you need to parse the returned PIDL
        [DllImport("shell32.dll", CharSet=CharSet.Unicode)]
        // static extern uint SHGetPathFromIDList(IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] 
        //StringBuilder pszPath);
        internal static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

        [DllImport("user32.dll", PreserveSig = true)]
        internal static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, string lParam);


        internal const uint COINIT_APARTMENTTHREADED = 0x2;      // Apartment model
        internal const uint COINIT_MULTITHREADED      = 0x0;      // OLE calls objects on any thread.
        internal const uint COINIT_DISABLE_OLE1DDE = 0x4;      // Don't use DDE for Ole1 support.
        internal const uint COINIT_SPEED_OVER_MEMORY = 0x8;      // Trade memory for speed.

        [DllImport("ole32.dll")]
        internal static extern IntPtr CoInitializeEx(IntPtr pvReserved, ulong dwCoInit);
        
        [DllImport("ole32.dll")]
        internal static extern IntPtr OleInitialize(IntPtr pvReserved);

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        internal class OPENFILENAME_32
        {
            public int lstructSize;
            public int hwndOwner;
            public int hInstance;
            public string lpstrFilter;
            public string lpstrCustomFilter;
            public int lMaxCustomFilter;
            public int lFilterIndex;
            public string lpstrFile;
            public int lMaxFile;
            public string lpstrFileTitle;
            public int lMaxFileTitle;
            public string lpstrInitialDir;
            public string lpstrTitle;
            public int lFlags;
            public ushort nFileOffset;
            public ushort nFileExtension;
            public string lpstrDefExt;
            public int lCustData;
            public IntPtr lpfHook;
            public int lpTemplateName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        internal class OPENFILENAME_64
        {
            public long lstructSize;
            public IntPtr hwndOwner;
            public int hInstance;
            public string lpstrFilter;
            public string lpstrCustomFilter;
            public int lMaxCustomFilter;
            public int lFilterIndex;
            public string lpstrFile;
            public int lMaxFile;
            public string lpstrFileTitle;
            public int lMaxFileTitle;
            public string lpstrInitialDir;
            public string lpstrTitle;
            public int lFlags;
            public ushort nFileOffset;
            public ushort nFileExtension;
            public string lpstrDefExt;
            public int lCustData;
            public IntPtr lpfHook;
            public int lpTemplateName;
        }

        internal const int OFN_READONLY = 0x00000001;
        internal const int OFN_OVERWRITEPROMPT = 0x00000002;
        internal const int OFN_HIDEREADONLY = 0x00000004;
        internal const int OFN_NOCHANGEDIR = 0x00000008;
        internal const int OFN_SHOWHELP = 0x00000010;
        internal const int OFN_ENABLEHOOK = 0x00000020;
        internal const int OFN_ENABLETEMPLATE = 0x00000040;
        internal const int OFN_ENABLETEMPLATEHANDLE = 0x00000080;
        internal const int OFN_NOVALIDATE = 0x00000100;
        internal const int OFN_ALLOWMULTISELECT = 0x00000200;
        internal const int OFN_EXTENSIONDIFFERENT = 0x00000400;
        internal const int OFN_PATHMUSTEXIST = 0x00000800;
        internal const int OFN_FILEMUSTEXIST = 0x00001000;
        internal const int OFN_CREATEPROMPT = 0x00002000;
        internal const int OFN_SHAREAWARE = 0x00004000;
        internal const int OFN_NOREADONLYRETURN = 0x00008000;
        internal const int OFN_NOTESTFILECREATE = 0x00010000;
        internal const int OFN_NONETWORKBUTTON = 0x00020000;
        internal const int OFN_NOLONGNAMES = 0x00040000;     // force no long names for 4.x modules
        internal const int OFN_EXPLORER = 0x00080000;     // new look commdlg
        internal const int OFN_NODEREFERENCELINKS = 0x00100000;
        internal const int OFN_LONGNAMES = 0x00200000;     // force long names for 3.x modules
        // OFN_ENABLEINCLUDENOTIFY and OFN_ENABLESIZING require
        // Windows 2000 or higher to have any effect.
        internal const int OFN_ENABLEINCLUDENOTIFY = 0x00400000;     // send include message to callback
        internal const int OFN_ENABLESIZING = 0x00800000;
        internal const int OFN_DONTADDTORECENT = 0x02000000;
        internal const int OFN_FORCESHOWHIDDEN = 0x10000000;    // Show All files including System and hidden files
        internal const int OFN_EX_NOPLACESBAR = 0x00000001;

        [DllImport("comdlg32.dll", EntryPoint = "GetOpenFileName", CharSet = CharSet.Auto, SetLastError=true)]
        internal static extern bool GetOpenFileName32([In, Out] OPENFILENAME_32 ofn);
        
        [DllImport("comdlg32.dll", EntryPoint="GetSaveFileName", CharSet=CharSet.Auto, SetLastError=true)]
        internal static extern bool GetSaveFileName32([In, Out] OPENFILENAME_32 ofn);

        [DllImport("comdlg32.dll", EntryPoint = "GetOpenFileName", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetOpenFileName64([In, Out] OPENFILENAME_64 ofn);

        [DllImport("comdlg32.dll", EntryPoint = "GetSaveFileName", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetSaveFileName64([In, Out] OPENFILENAME_64 ofn);

    }
}