// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure
{
    /// <summary>
    /// List of all supported CEF wrappers.
    /// </summary>
    public enum ChromelyCefWrapper
    {
        /// <summary>
        /// See: https://gitlab.com/xiliumhq/chromiumembedded/cefglue
        /// </summary>
        CefGlue,

        /// <summary>
        /// See: https://github.com/Chromely/Chromely
        /// </summary>
        Chromely
    }
}
