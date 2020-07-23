// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core;
using Chromely.Core.Network;
using System.Collections.Generic;
using Xilium.CefGlue;

namespace Chromely
{
    public class ChromelyInfo : IChromelyInfo
    {
        public IChromelyResponse GetInfo(string requestId)
        {
            var response = new ChromelyResponse(requestId);
            var infoItemDic = new Dictionary<string, string>
            {
                {
                    "divObjective",
                    "To build HTML5 desktop apps using embedded Chromium without WinForm or WPF. Uses Windows, Linux and MacOS native GUI API. It can be extended to use WinForm or WPF. Main form of communication with Chromium rendering process is via CEF Message Router, Ajax HTTP/XHR requests using custom schemes and domains."
                },
                {
                    "divPlatform",
                    "Cross-platform - Windows, Linux, MacOS. Built on CefGlue, CefSharp, NET Standard 2.0, .NET Core 3.0, .NET Framework 4.61 and above."
                },
                { "divVersion", CefRuntime.ChromeVersion }
            };

            response.ReadyState = (int)ReadyState.ResponseIsReady;
            response.Status = (int)System.Net.HttpStatusCode.OK;
            response.StatusText = "OK";
            response.Data = infoItemDic;

            return response;
        }
    }
}
