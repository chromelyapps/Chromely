#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.CefGlue.Winapi.Browser
{
    using System;
    using Xilium.CefGlue;

    public class LoadStartEventArgs : EventArgs
	{
		public LoadStartEventArgs(CefFrame frame)
		{
			Frame = frame;
		}

		public CefFrame Frame { get; private set; }
	}
}
