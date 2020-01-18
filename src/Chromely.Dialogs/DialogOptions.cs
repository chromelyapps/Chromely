using System;

namespace Chromely.Dialogs
{
    public class DialogOptions
    {
        public DialogIcon Icon { get; set; }
        public string Title { get; set; }

        public DialogOptions()
        {
            Icon = DialogIcon.None;
            Title = String.Empty;
        }
    }
}