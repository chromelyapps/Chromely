using System;
using System.Collections.Generic;
using System.Linq;
using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.ViewModel;
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Chromely.Dialogs.TestApp.ViewModels
{
    // ReSharper disable once UnusedMember.Global
    public class MessagesVm : ActiveViewModel
    {
        public string MessageText { get; set; }
        public DialogOptions Options { get; set; }

        public List<KeyValuePair<int, string>> DialogIcons { get; }

        public DialogResponse Response { get; set; }


        public MessagesVm(AppSession session) : base(session)
        {
            MessageText = "Hello Chromely !";
            Options = new DialogOptions
            {
                Title = "Dialogs.TestApp",
                Icon = DialogIcon.Information
            };
            DialogIcons = Enumerable.Range(0, 100)
                .Zip(Enum.GetNames(typeof(DialogIcon)), (ix, name) => new KeyValuePair<int, string>(ix, name))
                .ToList();
            Response = new DialogResponse();
        }

        [ActionMethod]
        public void TestMessageBox()
        {
            Response = ChromelyDialogs.MessageBox(MessageText, Options);
            NotifyPropertyChanged(nameof(Response));
        }
    }
}