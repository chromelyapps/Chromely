#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Winapi.Browser
{
    using System;

    public class ConsoleMessageEventArgs : EventArgs
	{
		public ConsoleMessageEventArgs(string message, string source, int line)
		{
			Message = message;
			Source = source;
			Line = line;
		}

		public string Message { get; private set; }
		public string Source { get; private set; }
		public int Line { get; private set; }
		public bool Handled { get; set; }
	}
}
