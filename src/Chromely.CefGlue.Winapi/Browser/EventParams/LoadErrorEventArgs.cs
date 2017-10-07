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

    internal sealed class LoadErrorEventArgs : EventArgs
	{
		public LoadErrorEventArgs(CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
		{
			Frame = frame;
			ErrorCode = errorCode;
			ErrorText = errorText;
			FailedUrl = failedUrl;
		}

		public string FailedUrl { get; private set; }

		public string ErrorText { get; private set; }

		public CefErrorCode ErrorCode { get; private set; }

		public CefFrame Frame { get; private set; }
	}
}
