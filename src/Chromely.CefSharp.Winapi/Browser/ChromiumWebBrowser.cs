// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromiumWebBrowser.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Chromely.CefSharp.Winapi.Browser.FrameHandlers;
using Chromely.CefSharp.Winapi.Browser.Internals;
using Chromely.Core.Infrastructure;
using global::CefSharp;
using global::CefSharp.Internals;

namespace Chromely.CefSharp.Winapi.Browser
{
    /// <summary>
    /// The chromium web browser.
    /// </summary>
    internal class ChromiumWebBrowser : IWebBrowserInternal
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly AbstractCefSettings _settings;

        /// <summary>
        /// The managed cef browser adapter
        /// </summary>
        private ManagedCefBrowserAdapter _managedCefBrowserAdapter;

        /// <summary>
        /// The browser
        /// </summary>
        private IBrowser _browser;

        /// <summary>
        /// A flag that indicates whether or not <see cref="InitializeFieldsAndCefIfRequired"/> has been called.
        /// </summary>
        private bool _initialized;

        /// <summary>
        /// Has the underlying Cef Browser been created (slightly different to initliazed in that
        /// the browser is initialized in an async fashion)
        /// </summary>
        private bool _browserCreated;

        /// <summary>
        /// The disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// The request context (we deliberately use a private variable so we can throw an exception if
        /// user attempts to set after browser created)
        /// </summary>
        private IRequestContext _requestContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromiumWebBrowser"/> class.
        /// </summary>
        /// <param name="settings">
        /// The Settings.
        /// </param>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="useLegacyJavascriptBindingEnabled">
        /// Flag to use whether Javascript binding should be used.
        /// </param>
        public ChromiumWebBrowser(AbstractCefSettings settings, string address,  bool useLegacyJavascriptBindingEnabled = true)
        {
            Address = address;
            _settings = settings;
            CefSharpSettings.LegacyJavascriptBindingEnabled = useLegacyJavascriptBindingEnabled;
            InitializeFieldsAndCefIfRequired();
        }

        #region  Event Properties

        /// <summary>
        /// Event handler that will get called when the resource load for a navigation fails or is canceled.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<LoadErrorEventArgs> LoadError;

        /// <summary>
        /// Event handler that will get called when the browser begins loading a frame. Multiple frames may be loading at the same
        /// time. Sub-frames may start or continue loading after the main frame load has ended. This method may not be called for a
        /// particular frame if the load request for that frame fails. For notification of overall browser load status use
        /// OnLoadingStateChange instead.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        /// <remarks>Whilst this may seem like a logical place to execute js, it's called before the DOM has been loaded, implement
        /// <see cref="IRenderProcessMessageHandler.OnContextCreated" /> as it's called when the underlying V8Context is created
        /// (Only called for the main frame at this stage)</remarks>
        public event EventHandler<FrameLoadStartEventArgs> FrameLoadStart;

        /// <summary>
        /// Event handler that will get called when the browser is done loading a frame. Multiple frames may be loading at the same
        /// time. Sub-frames may start or continue loading after the main frame load has ended. This method will always be called
        /// for all frames irrespective of whether the request completes successfully.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<FrameLoadEndEventArgs> FrameLoadEnd;

        /// <summary>
        /// Event handler that will get called when the Loading state has changed.
        /// This event will be fired twice. Once when loading is initiated either programmatically or
        /// by user action, and once when loading is terminated due to completion, cancellation of failure.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<LoadingStateChangedEventArgs> LoadingStateChanged;

        /// <summary>
        /// Event handler for receiving Javascript console messages being sent from web pages.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// (The exception to this is when your running with settings.MultiThreadedMessageLoop = false, then they'll be the same thread).
        /// </summary>
        public event EventHandler<ConsoleMessageEventArgs> ConsoleMessage;

        /// <summary>
        /// Event handler for changes to the status message.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang.
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// (The exception to this is when your running with settings.MultiThreadedMessageLoop = false, then they'll be the same thread).
        /// </summary>
        public event EventHandler<StatusMessageEventArgs> StatusMessage;

        /// <summary>
        /// Occurs when the browser address changed.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<AddressChangedEventArgs> AddressChanged;

        /// <summary>
        /// Occurs when the browser title changed.
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<TitleChangedEventArgs> TitleChanged;

        /// <summary>
        /// Occurs when [is browser initialized changed].
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI
        /// thread. It is unwise to block on this thread for any length of time as your browser will become unresponsive and/or hang..
        /// To access UI elements you'll need to Invoke/Dispatch onto the UI Thread.
        /// </summary>
        public event EventHandler<IsBrowserInitializedChangedEventArgs> IsBrowserInitializedChanged;

        #endregion Event Properties

        /// <summary>
        /// Gets or sets the browser settings.
        /// </summary>
        /// <value>The browser settings.</value>
        public BrowserSettings BrowserSettings { get; set; }

        /// <summary>
        /// Gets or sets the request context.
        /// </summary>
        /// <value>The request context.</value>
        public IRequestContext RequestContext
        {
            get => _requestContext;

            set
            {
                if (_browserCreated)
                {
                    throw new Exception("Browser has already been created. RequestContext must be" +
                                        "set before the underlying CEF browser is created.");
                }

                if (value != null && value.GetType() != typeof(RequestContext))
                {
                    throw new Exception($"RequestContext can only be of type {typeof(RequestContext)} or null");
                }

                _requestContext = value;
            }
        }

        /// <summary>
        /// Gets the browser adapter.
        /// </summary>
        /// <value>The browser adapter.</value>
        IBrowserAdapter IWebBrowserInternal.BrowserAdapter => _managedCefBrowserAdapter;

        /// <summary>
        /// Gets a value indicating whether is loading.
        /// </summary>
        public bool IsLoading { get; private set; }

        /// <summary>
        /// Gets the tooltip text.
        /// </summary>
        public string TooltipText { get; private set; }

        /// <summary>
        /// Gets the address.
        /// </summary>
        public string Address { get; private set; }

        #region Handler Properties

        /// <summary>
        /// Gets or sets the dialog handler.
        /// </summary>
        public IDialogHandler DialogHandler { get; set; }

        /// <summary>
        /// Gets or sets the js dialog handler.
        /// </summary>
        public IJsDialogHandler JsDialogHandler { get; set; }

        /// <summary>
        /// Gets or sets the keyboard handler.
        /// </summary>
        public IKeyboardHandler KeyboardHandler { get; set; }

        /// <summary>
        /// Gets or sets the request handler.
        /// </summary>
        public IRequestHandler RequestHandler { get; set; }

        /// <summary>
        /// Gets or sets the download handler.
        /// </summary>
        public IDownloadHandler DownloadHandler { get; set; }

        /// <summary>
        /// Gets or sets the load handler.
        /// </summary>
        public ILoadHandler LoadHandler { get; set; }

        /// <summary>
        /// Gets or sets the life span handler.
        /// </summary>
        public ILifeSpanHandler LifeSpanHandler { get; set; }

        /// <summary>
        /// Gets or sets the display handler.
        /// </summary>
        public IDisplayHandler DisplayHandler { get; set; }

        /// <summary>
        /// Gets or sets the menu handler.
        /// </summary>
        public IContextMenuHandler MenuHandler { get; set; }

        /// <summary>
        /// Gets or sets the render process message handler.
        /// </summary>
        public IRenderProcessMessageHandler RenderProcessMessageHandler { get; set; }

        /// <summary>
        /// Gets or sets the find handler.
        /// </summary>
        public IFindHandler FindHandler { get; set; }

        /// <summary>
        /// Gets or sets the focus handler.
        /// </summary>
        /// <remarks>If you need customized focus handling behavior for WinForms, the suggested
        /// best practice would be to inherit from DefaultFocusHandler and try to avoid
        /// needing to override the logic in OnGotFocus. The implementation in
        /// DefaultFocusHandler relies on very detailed behavior of how WinForms and
        /// Windows interact during window activation.
        /// </remarks>
        public IFocusHandler FocusHandler { get; set; }

        /// <summary>
        /// Gets or sets the drag handler.
        /// </summary>
        public IDragHandler DragHandler { get; set; }

        /// <summary>
        /// Gets or sets the resource handler factory.
        /// </summary>
        public IResourceHandlerFactory ResourceHandlerFactory { get; set; }

        #endregion Handler Properties

        /// <summary>
        /// Gets a value indicating whether can go forward.
        /// </summary>
        public bool CanGoForward { get; private set; }

        /// <summary>
        /// Gets a value indicating whether can go back.
        /// </summary>
        public bool CanGoBack { get; private set; }

        /// <summary>
        /// Gets a value indicating whether is browser initialized.
        /// </summary>
        public bool IsBrowserInitialized { get; private set; }

        /// <summary>
        /// Gets a value indicating whether can execute javascript in main frame.
        /// </summary>
        public bool CanExecuteJavascriptInMainFrame { get; private set; }

        /// <summary>
        /// Gets the javascript object repository.
        /// </summary>
        public IJavascriptObjectRepository JavascriptObjectRepository => _managedCefBrowserAdapter?.JavascriptObjectRepository;

        /// <summary>
        /// Gets or sets a value indicating whether this instance has parent.
        /// </summary>
        /// <value><c>true</c> if this instance has parent; otherwise, <c>false</c>.</value>
        bool IWebBrowserInternal.HasParent { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromiumWebBrowser"/> class.
        /// </summary>
        /// <param name="parent">The parent handle.</param>
        /// <param name="address">Start address.</param>
        public void CreateBrowser(IntPtr parent, string address)
        {
            if (((IWebBrowserInternal)this).HasParent == false)
            {
                if (IsBrowserInitialized == false || _browser == null)
                {
                    RequestContext = Cef.GetGlobalRequestContext();
                    _browserCreated = true;
                    var windowInfo = new WindowInfo();
                    windowInfo.SetAsChild(parent);
                    BrowserSettings.WebSecurity = CefState.Disabled;
                    _managedCefBrowserAdapter.CreateBrowser(windowInfo, BrowserSettings, (RequestContext)RequestContext, address);
                }
                else
                {
                    // If the browser already exists we'll reparent it to the new Handle
                    var browserHandle = _browser.GetHost().GetWindowHandle();
                    NativeMethodWrapper.SetWindowParent(browserHandle, parent);
                }

                Log.Info("Cef browser successfully created.");
            }
        }

        /// <summary>
        /// Loads the specified URL.
        /// </summary>
        /// <param name="url">The URL to be loaded.</param>
        public void Load(string url)
        {
            if (IsBrowserInitialized)
            {
                this.GetMainFrame().LoadUrl(url);
            }
            else
            {
                Address = url;
            }
        }

        /// <summary>
        /// The register js object.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="objectToBind">
        /// The object to bind.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <exception cref="Exception">
        /// Exception - Object Repository Null, Browser has likely been Disposed.
        /// </exception>
        public void RegisterJsObject(string name, object objectToBind, BindingOptions options = null)
        {
            if (!CefSharpSettings.LegacyJavascriptBindingEnabled)
            {
                throw new Exception(@"CefSharpSettings.LegacyJavascriptBindingEnabled is currently false,
                                    for legacy binding you must set CefSharpSettings.LegacyJavascriptBindingEnabled = true
                                    before registering your first object see https://github.com/cefsharp/CefSharp/issues/2246
                                    for details on the new binding options. If you perform cross-site navigations bound objects will
                                    no longer be registered and you will have to migrate to the new method.");
            }

            if (IsBrowserInitialized)
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }

            InitializeFieldsAndCefIfRequired();

            // Enable WCF if not already enabled
            CefSharpSettings.WcfEnabled = true;

            var objectRepository = _managedCefBrowserAdapter.JavascriptObjectRepository;

            if (objectRepository == null)
            {
                throw new Exception("Object Repository Null, Browser has likely been Disposed.");
            }

            objectRepository.Register(name, objectToBind, false, options);
        }

        /// <summary>
        /// The register async js object.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="objectToBind">
        /// The object to bind.
        /// </param>
        /// <param name="options">
        /// The options.
        /// </param>
        /// <exception cref="Exception">
        /// Exception - Object Repository Null, Browser has likely been Disposed.
        /// </exception>
        public void RegisterAsyncJsObject(string name, object objectToBind, BindingOptions options = null)
        {
            if (!CefSharpSettings.LegacyJavascriptBindingEnabled)
            {
                throw new Exception(@"CefSharpSettings.LegacyJavascriptBindingEnabled is currently false,
                                    for legacy binding you must set CefSharpSettings.LegacyJavascriptBindingEnabled = true
                                    before registering your first object see https://github.com/cefsharp/CefSharp/issues/2246
                                    for details on the new binding options. If you perform cross-site navigations bound objects will
                                    no longer be registered and you will have to migrate to the new method.");
            }

            if (IsBrowserInitialized)
            {
                throw new Exception("Browser is already initialized. RegisterJsObject must be" +
                                    "called before the underlying CEF browser is created.");
            }

            InitializeFieldsAndCefIfRequired();

            var objectRepository = _managedCefBrowserAdapter.JavascriptObjectRepository;

            if (objectRepository == null)
            {
                throw new Exception("Object Repository Null, Browser has likely been Disposed.");
            }

            objectRepository.Register(name, objectToBind, true, options);
        }

        /// <summary>
        /// The set size.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public void SetSize(int width, int height)
        {
            if (_initialized)
            {
                ResizeBrowser(width, height);
            }
        }

        /// <summary>
        /// Returns the current IBrowser Instance
        /// </summary>
        /// <returns>browser instance or null</returns>
        public IBrowser GetBrowser()
        {
            this.ThrowExceptionIfBrowserNotInitialized();

            return _browser;
        }

        /// <summary>
        /// The focus.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Focus()
        {
            return true;
        }

        #region Event Handlers

        /// <summary>
        /// Called after browser created.
        /// </summary>
        /// <param name="browser">The browser.</param>
        void IWebBrowserInternal.OnAfterBrowserCreated(IBrowser browser)
        {
            _browser = browser;
            IsBrowserInitialized = true;

            if (!string.IsNullOrEmpty(Address))
            {
                browser.MainFrame.LoadUrl(Address);
            }

            // Register browser 
            var frameHandler = new CefSharpFrameHandler(browser);
            IoC.RegisterInstance(typeof(CefSharpFrameHandler), typeof(CefSharpFrameHandler).FullName, frameHandler);

            IsBrowserInitializedChanged?.Invoke(this, new IsBrowserInitializedChangedEventArgs(IsBrowserInitialized));
        }

        /// <summary>
        /// Sets the address.
        /// </summary>
        /// <param name="args">The <see cref="AddressChangedEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.SetAddress(AddressChangedEventArgs args)
        {
            Address = args.Address;
            AddressChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Sets the loading state change.
        /// </summary>
        /// <param name="args">The <see cref="LoadingStateChangedEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.SetLoadingStateChange(LoadingStateChangedEventArgs args)
        {
            CanGoBack = args.CanGoBack;
            CanGoForward = args.CanGoForward;
            IsLoading = args.IsLoading;

            LoadingStateChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <param name="args">The <see cref="TitleChangedEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.SetTitle(TitleChangedEventArgs args)
        {
            TitleChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Sets the tooltip text.
        /// </summary>
        /// <param name="tooltipText">The tooltip text.</param>
        void IWebBrowserInternal.SetTooltipText(string tooltipText)
        {
            TooltipText = tooltipText;
        }

        /// <summary>
        /// Handles the <see cref="E:FrameLoadStart" /> event.
        /// </summary>
        /// <param name="args">The <see cref="FrameLoadStartEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnFrameLoadStart(FrameLoadStartEventArgs args)
        {
            FrameLoadStart?.Invoke(this, args);
        }

        /// <summary>
        /// Handles the <see cref="E:FrameLoadEnd" /> event.
        /// </summary>
        /// <param name="args">The <see cref="FrameLoadEndEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnFrameLoadEnd(FrameLoadEndEventArgs args)
        {
            FrameLoadEnd?.Invoke(this, args);
        }

        /// <summary>
        /// Handles the <see cref="E:ConsoleMessage" /> event.
        /// </summary>
        /// <param name="args">The <see cref="ConsoleMessageEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnConsoleMessage(ConsoleMessageEventArgs args)
        {
            ConsoleMessage?.Invoke(this, args);
        }

        /// <summary>
        /// Handles the <see cref="E:StatusMessage" /> event.
        /// </summary>
        /// <param name="args">The <see cref="StatusMessageEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnStatusMessage(StatusMessageEventArgs args)
        {
            StatusMessage?.Invoke(this, args);
        }

        /// <summary>
        /// Handles the <see cref="E:LoadError" /> event.
        /// </summary>
        /// <param name="args">The <see cref="LoadErrorEventArgs"/> instance containing the event data.</param>
        void IWebBrowserInternal.OnLoadError(LoadErrorEventArgs args)
        {
            LoadError?.Invoke(this, args);
        }

        /// <summary>
        /// The set can execute javascript on main frame.
        /// </summary>
        /// <param name="canExecute">
        /// The can execute.
        /// </param>
        void IWebBrowserInternal.SetCanExecuteJavascriptOnMainFrame(bool canExecute)
        {
            CanExecuteJavascriptInMainFrame = canExecute;
        }

        #endregion  Event Handlers

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.Control" /> and its child controls and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            IsBrowserInitialized = false;
            Cef.RemoveDisposable(this);

            if (disposing)
            {
                FreeUnmanagedResources();
            }

            // Don't maintain a reference to event listeners any longer:
            LoadError = null;
            FrameLoadStart = null;
            FrameLoadEnd = null;
            LoadingStateChanged = null;
            ConsoleMessage = null;
            StatusMessage = null;
            AddressChanged = null;
            TitleChanged = null;
            IsBrowserInitializedChanged = null;

            // Release reference to handlers, make sure this is done after we dispose _managedCefBrowserAdapter
            // otherwise the ILifeSpanHandler.DoClose will not be invoked.
            this.SetHandlersToNull();

            _disposed = true;
        }

        /// <summary>
        /// Required for designer support - this method cannot be inlined as the designer
        /// will attempt to load libcef.dll and will subsiquently throw an exception.
        /// </summary>
        private void InitializeFieldsAndCefIfRequired()
        {
            if (!_initialized)
            {
                if (!Cef.IsInitialized && !Cef.Initialize(_settings))
                {
                    throw new InvalidOperationException("Cef::Initialize() failed");
                }

                Cef.AddDisposable(this);

                if (ResourceHandlerFactory == null)
                {
                    ResourceHandlerFactory = new DefaultResourceHandlerFactory();
                }

                if (BrowserSettings == null)
                {
                    BrowserSettings = new BrowserSettings();
                }

                _managedCefBrowserAdapter = new ManagedCefBrowserAdapter(this, false);
                _initialized = true;
            }
        }

        /// <summary>
        /// The resize browser.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        private void ResizeBrowser(int width, int height)
        {
            if (IsBrowserInitialized)
            {
                _managedCefBrowserAdapter.Resize(width, height);
            }
        }

        /// <summary>
        /// Required for designer support - this method cannot be inlined as the designer
        /// will attempt to load libcef.dll and will subsiquently throw an exception.
        /// </summary>
        private void FreeUnmanagedResources()
        {
            _browser = null;

            if (BrowserSettings != null)
            {
                BrowserSettings.Dispose();
                BrowserSettings = null;
            }

            if (_managedCefBrowserAdapter != null)
            {
                _managedCefBrowserAdapter.Dispose();
                _managedCefBrowserAdapter = null;
            }
        }
    }
}