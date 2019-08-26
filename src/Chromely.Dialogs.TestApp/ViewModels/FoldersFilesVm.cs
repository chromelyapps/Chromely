using System;
using System.Collections.Generic;
using System.Linq;
using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.ViewModel;
// ReSharper disable UnusedMember.Global

namespace Chromely.Dialogs.TestApp.ViewModels
{
    // ReSharper disable once UnusedMember.Global
    public class FoldersFilesVm : ActiveViewModel
    {
        public string MessageText { get; set; }
        public FileDialogOptions Options { get; set; }

        public List<KeyValuePair<int, string>> DialogIcons { get; }

        public string Directory { get; set; }
        public string FileName { get; set; }

        public DialogResponse Response { get; set; }


        public FoldersFilesVm(AppSession session) : base(session)
        {
            MessageText = "Hello Chromely !";
            FileName = "Test.txt";
            Options = new FileDialogOptions
            {
                Title = "Dialog Title",
                Directory = AppDomain.CurrentDomain.BaseDirectory
            };
            DialogIcons = Enumerable.Range(0, 100)
                .Zip(Enum.GetNames(typeof(DialogIcon)), (ix, name) => new KeyValuePair<int, string>(ix, name))
                .ToList();
            Response = new DialogResponse();
        }

        [ActionMethod]
        public void TestSelectFolder()
        {
            Response = ChromelyDialogs.SelectFolder(MessageText, Options);
            NotifyPropertyChanged(nameof(Response));
        }

        [ActionMethod]
        public void TestFileOpen()
        {
            Response = ChromelyDialogs.FileOpen(MessageText, Options);
            NotifyPropertyChanged(nameof(Response));
        }

        [ActionMethod]
        public void TestFileSave()
        {
            Response = ChromelyDialogs.FileSave(MessageText, FileName, Options);
            NotifyPropertyChanged(nameof(Response));
        }

    }
}
