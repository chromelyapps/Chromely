#region Port Info
/**
 * This is a port from CefGlue.WindowsForms sample of . Mostly provided as-is. 
 * For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
 **/
#endregion

namespace Chromely.Browser.EventParams
{
    using System;
    using Xilium.CefGlue;

    public class AddressChangedEventArgs : EventArgs
	{
		public AddressChangedEventArgs(CefFrame frame, string address)
		{
			Address = address;
			Frame = frame;
		}

		public string Address { get; private set; }

		public CefFrame Frame { get; private set; }
	}
}
