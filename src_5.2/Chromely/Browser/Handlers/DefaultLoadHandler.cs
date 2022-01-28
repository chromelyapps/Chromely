// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

/// <summary>
/// Default implementation of <see cref="CefLoadHandler"/>.
/// </summary>
public class DefaultLoadHandler : CefLoadHandler
{
    protected readonly IChromelyConfiguration _config;

    /// <summary>
    /// The CefGlueBrowser object.
    /// </summary>
    protected ChromiumBrowser? _browser;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultLoadHandler"/> class.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    /// <param name="window">Instance of <see cref="IChromelyWindow"/>.</param>
    public DefaultLoadHandler(IChromelyConfiguration config, IChromelyWindow window)
    {
        _config = config;
        _browser = window as ChromiumBrowser;
    }

    /// <summary>
    /// Gets or sets the browser.
    /// </summary>
    public ChromiumBrowser? Browser
    {
        get { return _browser; }
        set { _browser = value; }
    }

    /// <inheritdoc/>
    protected override void OnLoadEnd(CefBrowser browser, CefFrame frame, int httpStatusCode)
    {
        if (_browser is not null)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnFrameLoadEnd(new FrameLoadEndEventArgs(frame, httpStatusCode)));
        }
    }

    /// <inheritdoc/>
    protected override void OnLoadError(CefBrowser browser, CefFrame frame, CefErrorCode errorCode, string errorText, string failedUrl)
    {
        if (_browser is not null)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnLoadError(new LoadErrorEventArgs(frame, errorCode, errorText, failedUrl)));
        }
    }

    /// <inheritdoc/>
    protected override void OnLoadStart(CefBrowser browser, CefFrame frame, CefTransitionType transitionType)
    {
        if (_browser is not null)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnFrameLoadStart(new FrameLoadStartEventArgs(frame)));
        }
    }

    /// <inheritdoc/>
    protected override void OnLoadingStateChange(CefBrowser browser, bool isLoading, bool canGoBack, bool canGoForward)
    {
        if (_browser is not null)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnLoadingStateChange(new LoadingStateChangedEventArgs(isLoading, canGoBack, canGoForward)));
        }
    }
}