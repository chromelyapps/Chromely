using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.ViewModel;

namespace Chromely.Dialogs.TestApp.ViewModels
{
    public class MessagesVm : ActiveViewModel
    {
        public string MessageText { get; set; }
        public DialogOptions Options { get; set; }

        public MessagesVm(AppSession session) : base(session)
        {
            MessageText = "Hello Chromely !";
        }

        [ActionMethod]
        public void MessageBox()
        {
            ChromelyDialogs.MessageBox(MessageText, Options);
        }
    }
}