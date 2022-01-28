// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Browser;

/// <summary>
/// Default implementation of <see cref="CefDisplayHandler"/>.
/// </summary>
public class DefaultDisplayHandler : CefDisplayHandler
{
    protected readonly IChromelyConfiguration _config;
    protected ChromiumBrowser? _browser;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultDisplayHandler"/>.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    /// <param name="window">Instance of <see cref="IChromelyWindow"/>.</param>
    public DefaultDisplayHandler(IChromelyConfiguration config, IChromelyWindow window)
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
    protected override void OnTitleChange(CefBrowser browser, string title)
    {
        if (_browser is not null)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnTitleChanged(new TitleChangedEventArgs(title)));
        }
    }

    /// <inheritdoc/>
    protected override void OnAddressChange(CefBrowser browser, CefFrame frame, string url)
    {
        if (frame.IsMain && _browser is not null)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnAddressChanged(new AddressChangedEventArgs(frame, url)));
        }
    }

    /// <inheritdoc/>
    protected override void OnStatusMessage(CefBrowser browser, string value)
    {
        if (_browser is not null)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnStatusMessage(new StatusMessageEventArgs(value)));
        }
    }

    /// <inheritdoc/>
    protected override bool OnConsoleMessage(CefBrowser browser, CefLogSeverity level, string message, string source, int line)
    {
        var evntArgs = new ConsoleMessageEventArgs(message, source, line);
        if (_browser is not null)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnConsoleMessage(evntArgs));
        }

        return evntArgs.Handled;
    }

    /// <inheritdoc/>
    protected override bool OnTooltip(CefBrowser browser, string text)
    {
        var evntArgs = new TooltipEventArgs(text);
        if (_browser is not null)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnTooltip(evntArgs));
        }

        return evntArgs.Handled;
    }
}