using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Chromely.Core.Host;
// ReSharper disable MemberCanBePrivate.Global

// ReSharper disable CommentTypo

namespace Chromely.Dialogs.Windows
{
    public class WindowsDialogs : IChromelyDialogs
    {
        public void Init(IChromelyWindow window)
        {
        }
        
        public DialogResponse MessageBox(string message, DialogOptions options)
        {
            var type = WindowsInterop.MB_SYSTEMMODAL | WindowsInterop.MB_SETFOREGROUND;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (options.Icon)
            {
                case DialogIcon.Question:
                    type |= WindowsInterop.MB_ICONQUESTION;
                    break;
                case DialogIcon.Information:
                    type |= WindowsInterop.MB_ICONINFORMATION;
                    break;
                case DialogIcon.Warning:
                    type |= WindowsInterop.MB_ICONWARNING;
                    break;
                case DialogIcon.Error:
                    type |= WindowsInterop.MB_ICONERROR;
                    break;
            }
            var ret = WindowsInterop.MessageBox(IntPtr.Zero, message, options.Title, type);
            return new DialogResponse { IsCanceled = ret == WindowsInterop.IDCANCEL };
        }


        private static string _initialPath;
        private int OnBrowseEvent(IntPtr hWnd, int msg, IntPtr lp, IntPtr lpData)
        {
            switch(msg)
            {
                case WindowsInterop.BFFM_INITIALIZED: // Required to set initialPath
                {
                    //Win32.SendMessage(new HandleRef(null, hWnd), BFFM_SETSELECTIONA, 1, lpData);
                    // Use BFFM_SETSELECTIONW if passing a Unicode string, i.e. native CLR Strings.
                    if (!string.IsNullOrEmpty(_initialPath))
                    {
                        WindowsInterop.SendMessage(new HandleRef(null, hWnd), WindowsInterop.BFFM_SETSELECTIONW, 1, _initialPath);
                    }
                    break;
                }
                case WindowsInterop.BFFM_SELCHANGED:
                {
                    var pathPtr = Marshal.AllocHGlobal(260 * Marshal.SystemDefaultCharSize);
                    if (WindowsInterop.SHGetPathFromIDList(lp, pathPtr))
                        WindowsInterop.SendMessage(new HandleRef(null, hWnd), WindowsInterop.BFFM_SETSTATUSTEXTW, 0, pathPtr);
                    Marshal.FreeHGlobal(pathPtr);
                    break;
                }
            }

            return 0;
        }

        public DialogResponse SelectFolder(string message, FileDialogOptions options)
        {
            var result = new DialogResponse { IsCanceled = true }; 
            var th = new Thread(() =>
            {
                WindowsInterop.OleInitialize(IntPtr.Zero);
                result = SelectFolderInternal(message, options);
            })
            {
#pragma warning disable 618
                ApartmentState = ApartmentState.STA
#pragma warning restore 618
            };
            th.Start();
            th.Join();
            return result;
        }

        private DialogResponse SelectFolderInternal(string message, FileDialogOptions options)
        {
            if (Directory.Exists(options.Directory))
            {
                _initialPath = options.Directory;
            }
            var pIdList = IntPtr.Zero;
            var sb = new StringBuilder(256);
            var bufferAddress = Marshal.AllocHGlobal(256);
            var bi = new WindowsInterop.BROWSEINFO
            {
                hwndOwner = IntPtr.Zero,
                pidlRoot = IntPtr.Zero,
                pszDisplayName = message,
                lpszTitle = options.Title,
                ulFlags = WindowsInterop.BIF_NEWDIALOGSTYLE | WindowsInterop.BIF_SHAREABLE,
                lpfn = OnBrowseEvent,
                lParam = IntPtr.Zero,
                iImage = 0
            };
            
            try
            {
                
                pIdList = WindowsInterop.SHBrowseForFolder(ref bi);
                if (pIdList == IntPtr.Zero)
                {
                    return new DialogResponse { IsCanceled = true };
                }
                if (true != WindowsInterop.SHGetPathFromIDList(pIdList, bufferAddress))
                {
                    return new DialogResponse { Value = string.Empty };
                }
                sb.Append(Marshal.PtrToStringAuto(bufferAddress));
            }
            finally
            {
                // Caller is responsible for freeing this memory.
                Marshal.FreeCoTaskMem(pIdList);
            }

            return new DialogResponse { Value = sb.ToString() };
        }

        private static string ExtPattern(IEnumerable<string> patterns)
        {
            var filters = new List<string>();

            foreach (var pattern in patterns.Select(p => p.Trim()))
            {
                if (pattern.StartsWith("."))
                {
                    filters.Add("*" + pattern);
                }
                else if (!pattern.StartsWith("*"))
                {
                    filters.Add("*." + pattern);
                }
                else
                {
                    filters.Add(pattern);
                }
            }

            return string.Join(";", filters);
        }

        public DialogResponse FileOpen(string message, FileDialogOptions options)
        {
            return Environment.Is64BitProcess
                ? FileOpen64(message, options)
                : FileOpen32(message, options);
        }
        public DialogResponse FileOpen32(string message, FileDialogOptions options)
        {
            var ofn = new WindowsInterop.OPENFILENAME_32();
            ofn.lstructSize = Marshal.SizeOf(ofn);

            if (Directory.Exists(options.Directory))
            {
                ofn.lpstrInitialDir = options.Directory;
            }

            // The first string in each pair is a display string that describes the filter (for example, "Text Files"),
            // and the second string specifies the filter pattern (for example, ".TXT").
            // To specify multiple filter patterns for a single display string,
            // use a semicolon to separate the patterns (for example, ".TXT;.DOC;.BAK").
            // A pattern string can be a combination of valid file name characters and the asterisk (*) wildcard character.
            // Do not include spaces in the pattern string.
            ofn.lpstrFilter = options.Filters
                                  .Select(f => $"{f.Name}\0{ExtPattern(f.Patterns)}\0")
                                  .Aggregate("", (s1, s2) => s1 + s2)
                              + "\0";

            ofn.lpstrFile = new string(' ', 4096);
            ofn.lMaxFile = ofn.lpstrFile.Length;

            ofn.lpstrTitle = options.Title;
            ofn.lMaxFileTitle = ofn.lpstrTitle.Length;

            ofn.lFlags = WindowsInterop.OFN_EXPLORER;
            if (options.MustExist)
            {
                ofn.lFlags |= WindowsInterop.OFN_PATHMUSTEXIST | WindowsInterop.OFN_FILEMUSTEXIST;
            }

            var ok = WindowsInterop.GetOpenFileName32(ofn);
            return new DialogResponse { IsCanceled = !ok, Value = ofn.lpstrFile };
        }

        public static DialogResponse FileOpen64(string message, FileDialogOptions options)
        {
            var ofn = new WindowsInterop.OPENFILENAME_64();
            ofn.lstructSize = Marshal.SizeOf(ofn);
            
            if (Directory.Exists(options.Directory))
            {
                ofn.lpstrInitialDir = options.Directory;
            }

            ofn.lpstrFilter = options.Filters
                .Select(f => $"{f.Name}\0{ExtPattern(f.Patterns)}\0")
                .Aggregate("", (s1, s2) => s1 + s2)
                + "\0";

            ofn.lpstrFile = new string(' ', 4096);
            ofn.lMaxFile = ofn.lpstrFile.Length;
            
            ofn.lpstrTitle = options.Title;
            ofn.lMaxFileTitle = ofn.lpstrTitle.Length;

            ofn.lFlags = WindowsInterop.OFN_EXPLORER;
            if (options.MustExist)
            {
                ofn.lFlags |= WindowsInterop.OFN_PATHMUSTEXIST | WindowsInterop.OFN_FILEMUSTEXIST;
            }
            
            var ok = WindowsInterop.GetOpenFileName64(ofn);
            return new DialogResponse { IsCanceled = !ok, Value = ofn.lpstrFile };
        }

        public DialogResponse FileSave(string message, string fileName, FileDialogOptions options)
        {
            return Environment.Is64BitProcess
                ? FileSave64(message, fileName, options)
                : FileSave32(message, fileName, options);
        }
        public static DialogResponse FileSave32(string message, string fileName, FileDialogOptions options)
        {
            var ofn = new WindowsInterop.OPENFILENAME_32();
            ofn.lstructSize = Marshal.SizeOf(ofn);
            
            if (Directory.Exists(options.Directory))
            {
                ofn.lpstrInitialDir = options.Directory;
            }

            ofn.lpstrFilter = options.Filters
                                  .Select(f => $"{f.Name}\0{ExtPattern(f.Patterns)}\0")
                                  .Aggregate("", (s1, s2) => s1 + s2)
                              + "\0";

            ofn.lpstrFile = (fileName ?? "") + "\0" + new string(' ', 4096); 
            ofn.lMaxFile = 4096;
            
            ofn.lpstrTitle = options.Title;
            ofn.lMaxFileTitle = ofn.lpstrTitle.Length;

            ofn.lFlags = WindowsInterop.OFN_EXPLORER;
            if (options.MustExist)
            {
                ofn.lFlags |= WindowsInterop.OFN_PATHMUSTEXIST | WindowsInterop.OFN_FILEMUSTEXIST;
            }
            if(options.ConfirmOverwrite)
            {
                ofn.lFlags |= WindowsInterop.OFN_OVERWRITEPROMPT;
            }

            var ok = WindowsInterop.GetSaveFileName32(ofn);
            return new DialogResponse { IsCanceled = !ok, Value = ofn.lpstrFile };
        }
        public static DialogResponse FileSave64(string message, string fileName, FileDialogOptions options)
        {
            var ofn = new WindowsInterop.OPENFILENAME_64();
            ofn.lstructSize = Marshal.SizeOf(ofn);

            if (Directory.Exists(options.Directory))
            {
                ofn.lpstrInitialDir = options.Directory;
            }

            ofn.lpstrFilter = options.Filters
                                  .Select(f => $"{f.Name}\0{ExtPattern(f.Patterns)}\0")
                                  .Aggregate("", (s1, s2) => s1 + s2)
                              + "\0";

            ofn.lpstrFile = (fileName ?? "") + "\0" + new string(' ', 4096);
            ofn.lMaxFile = 4096;

            ofn.lpstrTitle = options.Title;
            ofn.lMaxFileTitle = ofn.lpstrTitle.Length;

            ofn.lFlags = WindowsInterop.OFN_EXPLORER;
            if (options.MustExist)
            {
                ofn.lFlags |= WindowsInterop.OFN_PATHMUSTEXIST | WindowsInterop.OFN_FILEMUSTEXIST;
            }
            if (options.ConfirmOverwrite)
            {
                ofn.lFlags |= WindowsInterop.OFN_OVERWRITEPROMPT;
            }

            var ok = WindowsInterop.GetSaveFileName64(ofn);
            return new DialogResponse { IsCanceled = !ok, Value = ofn.lpstrFile };
        }

    }
}