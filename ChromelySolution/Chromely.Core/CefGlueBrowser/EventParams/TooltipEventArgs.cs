#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.Core.CefGlueBrowser
{
    using System;

    public class TooltipEventArgs : EventArgs
	{
		public TooltipEventArgs(string text)
		{
			Text = text;
		}

		public string Text { get; private set; }
		public bool Handled { get; set; }
	}
}
