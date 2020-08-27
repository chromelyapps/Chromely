
# Registering Custom Handlers

All CEF/CefGlue handlers can be configured.

Custom handlers can be registered in [ChromelyApp](https://github.com/chromelyapps/Chromely/blob/f147ba0f7a3a9b18dbc8d6de1598eee1b0644d0b/src/Chromely.Core/ChromelyApp.cs#L113) Configure method.

The following handlers can be customized and registered:

- [CefLifeSpanHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/CefGlue/Classes.Handlers/CefLifeSpanHandler.cs)
- [CefLoadHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/CefGlue/Classes.Handlers/CefLoadHandler.cs)
- [CefRequestHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/CefGlue/Classes.Handlers/CefRequestHandler.cs)
- [CefDisplayHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/CefGlue/Classes.Handlers/CefDisplayHandler.cs)
- [CefContextMenuHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/CefGlue/Classes.Handlers/CefContextMenuHandler.cs)
- [CefFocusHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/CefGlue/Classes.Handlers/CefFocusHandler.cs)
- [CefKeyboardHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/CefGlue/Classes.Handlers/CefKeyboardHandler.cs)
- [CefJSDialogHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/CefGlue/Classes.Handlers/CefJSDialogHandler.cs)
- [CefDialogHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/CefGlue/Classes.Handlers/CefDialogHandler.cs)
- [CefDragHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/CefGlue/Classes.Handlers/CefDragHandler.cs)
- [CefDownloadHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/CefGlue/Classes.Handlers/CefDownloadHandler.cs)
- [CefFindHandler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/CefGlue/Classes.Handlers/CefFindHandler.cs)

For example to register [CefContext Menu Handler](https://github.com/chromelyapps/Chromely/blob/master/src/Chromely.CefGlue/CefGlue/Classes.Handlers/CefContextMenuHandler.cs):

````csharp
    public class CusomChromelyApp : ChromelyBasicApp
    {
        public override void Configure(IChromelyContainer container)
        {
            base.Configure(container);
            container.RegisterSingleton(typeof(IChromelyCustomHandler), Guid.NewGuid().ToString(), typeof(CustomContextMenuHandler));
        }
    }

    public class CustomContextMenuHandler : CefContextMenuHandler
    {
    }
````
