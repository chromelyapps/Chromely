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

    public class LoadEndEventArgs : EventArgs
	{
		public LoadEndEventArgs(CefFrame frame, int httpStatusCode)
		{
			Frame = frame;
			HttpStatusCode = httpStatusCode;
		}

		public int HttpStatusCode { get; private set; }

		public CefFrame Frame { get; private set; }
	}
}
