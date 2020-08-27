// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure
{
    public enum CefEventKey
    {
        None,
        FrameLoadStart,
        AddressChanged,
        TitleChanged,
        FrameLoadEnd,
        LoadingStateChanged,
        ConsoleMessage,
        StatusMessage,
        LoadError,
        TooltipChanged,
        BeforeClose,
        BeforePopup,
        PluginCrashed,
        RenderProcessTerminated
    }
}
