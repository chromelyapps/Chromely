using System.Collections.Generic;
using System.Threading;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Chromely.Dialogs
{
    public class FileDialogOptions : DialogOptions
    {
        public bool MustExist { get; set; }
        public bool ConfirmOverwrite { get; set; }
        public string Directory { get; set; }
        public List<FileFilter> Filters { get; private set; }

        public FileDialogOptions()
        {
            // ReSharper disable StringLiteralTypo
            var allFiles = Thread.CurrentThread.CurrentUICulture.Name.StartsWith("de")
                ? "Alle Dateien (*.*)"
                : "All files (*.*)";
            // ReSharper restore StringLiteralTypo
            Filters = new List<FileFilter> {new FileFilter {Name = allFiles, Patterns = new []{ "*.* "} }};
        }
    }
}