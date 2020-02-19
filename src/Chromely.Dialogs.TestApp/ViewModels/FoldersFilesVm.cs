using System;
using System.Collections.Generic;
using System.Linq;
using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.ViewModel;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Chromely.Dialogs.TestApp.ViewModels
{
    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once UnusedType.Global
    public class FoldersFilesVm : ActiveViewModel
    {
        public string MessageText { get; set; }


        private readonly FileDialogOptions _options;

        public string OptionsTitle
        {
            get => _options.Title;
            set => _options.Title = value;
        }
        public string OptionsDirectory
        {
            get => _options.Directory;
            set => _options.Directory = value;
        }
        

        public List<KeyValuePair<int, string>> DialogIcons { get; }

        public string Directory { get; set; }
        public string FileName { get; set; }

        public DialogResponse Response { get; set; }


        public FoldersFilesVm(AppSession session) : base(session)
        {
            MessageText = "Hello Chromely !";
            FileName = "Test.txt";
            _options = new FileDialogOptions
            {
                Title = "Dialog Title",
                Directory = AppDomain.CurrentDomain.BaseDirectory
            };
            _options.Filters.Add(new FileFilter
            {
                Name = "Excel files", 
                Patterns = new []{ "csv", ".xls", "*.xslx" }
            });
            DialogIcons = Enumerable.Range(0, 100)
                .Zip(Enum.GetNames(typeof(DialogIcon)), (ix, name) => new KeyValuePair<int, string>(ix, name))
                .ToList();
            Response = new DialogResponse();
        }

        [ActionMethod]
        public void TestSelectFolder()
        {
            Response = ChromelyDialogs.SelectFolder(MessageText, _options);
            NotifyPropertyChanged(nameof(Response));
        }

        [ActionMethod]
        public void TestFileOpen()
        {
            Response = ChromelyDialogs.FileOpen(MessageText, _options);
            NotifyPropertyChanged(nameof(Response));
        }

        [ActionMethod]
        public void TestFileSave()
        {
            Response = ChromelyDialogs.FileSave(MessageText, FileName, _options);
            NotifyPropertyChanged(nameof(Response));
        }

    }
}
