#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.Browser.EventParams
{
    using System;

    public class LoadingStateChangeEventArgs : EventArgs
	{
		public LoadingStateChangeEventArgs(bool isLoading, bool canGoBack, bool canGoForward)
		{
			IsLoading = isLoading;
			CanGoBack = canGoBack;
			CanGoForward = canGoForward;
		}

		public bool IsLoading { get; private set; } 
		public bool CanGoBack { get; private set; }
		public bool CanGoForward { get; private set; } 
	}
}
