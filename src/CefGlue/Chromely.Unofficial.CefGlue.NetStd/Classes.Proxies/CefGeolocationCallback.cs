namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;

    /// <summary>
    /// Callback interface used for asynchronous continuation of geolocation
    /// permission requests.
    /// </summary>
    public sealed unsafe partial class CefGeolocationCallback
    {
        /// <summary>
        /// Call to allow or deny geolocation access.
        /// </summary>
        public void Continue(bool allow)
        {
            // FIXME: it is can be executed only once - so may be make it self-disposable ?

            cef_geolocation_callback_t.cont(_self, allow ? 1 : 0);
        }
    }
}
