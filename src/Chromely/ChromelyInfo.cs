// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely;

/// <inheritdoc/>
public class ChromelyInfo : IChromelyInfo
{
    /// <inheritdoc/>
    public IChromelyResponse GetInfo(string requestId)
    {
        var response = new ChromelyResponse(requestId);
        var infoItemDic = new Dictionary<string, string>
            {
                {
                    "objective",
                    "To build HTML5 desktop apps using embedded Chromium without WinForm or WPF. Uses Windows, Linux and MacOS native GUI API. It can be extended to use WinForm or WPF. Main form of communication with Chromium rendering process is via CEF Message Router, Ajax HTTP/XHR requests using custom schemes and domains."
                },
                {
                    "platform",
                    "Cross-platform - Windows, Linux, MacOS. Built on CefGlue, CefSharp, NET Standard 2.0, .NET 6.0, .NET Framework 4.61 and above."
                },
                { "version", CefRuntime.ChromeVersion }
            };

        response.ReadyState = (int)ReadyState.ResponseIsReady;
        response.Status = (int)System.Net.HttpStatusCode.OK;
        response.StatusText = "OK";
        response.Data = infoItemDic;

        return response;
    }

    /// <inheritdoc/>
    public IDictionary<string, string> GetInfo()
    {
        return new Dictionary<string, string>
            {
                {
                    "objective",
                    "To build HTML5 desktop apps using embedded Chromium without WinForm or WPF. Uses Windows, Linux and MacOS native GUI API. It can be extended to use WinForm or WPF. Main form of communication with Chromium rendering process is via CEF Message Router, Ajax HTTP/XHR requests using custom schemes and domains."
                },
                {
                    "platform",
                    "Cross-platform - Windows, Linux, MacOS. Built on CefGlue, CefSharp, NET Standard 2.0, .NET 6.0, .NET Framework 4.61 and above."
                },
                { "version", CefRuntime.ChromeVersion }
            };
    }
}