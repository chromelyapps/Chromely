/**
MIT License

Copyright (c) 2017 Kola Oyewumi

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
namespace Chromely.Core.Helpers
{
    using System;
    using System.Collections.Generic;

    public static class CefHandlerDummyTypes
    {
        public interface ILifeSpanHandler {}
        public interface ILoadHandler {}
        public interface IRequestHandler {}
        public interface IDisplayHandler {}
        public interface IContextMenuHandler {}
        public interface IFocusHandler {}
        public interface IKeyboardHandler {}
        public interface IJSDialogHandler {}
        public interface IDialogHandler {}
        public interface IDragHandler {}
        public interface IGeolocationHandler {}
        public interface IDownloadHandler {}
        public interface IFindHandler {}

        public static Type GetHandlerType(CefHandlerKey key)
        {
            switch (key)
            {
                case CefHandlerKey.LifeSpanHandler:
                    return typeof(ILifeSpanHandler);

                case CefHandlerKey.LoadHandler:
                    return typeof(ILoadHandler);

                case CefHandlerKey.RequestHandler:
                    return typeof(IRequestHandler);

                case CefHandlerKey.DisplayHandler:
                    return typeof(IDisplayHandler);

                case CefHandlerKey.ContextMenuHandler:
                    return typeof(IContextMenuHandler);

                case CefHandlerKey.FocusHandler:
                    return typeof(IFocusHandler);

                case CefHandlerKey.KeyboardHandler:
                    return typeof(IKeyboardHandler);

                case CefHandlerKey.JSDialogHandler:
                    return typeof(IJSDialogHandler);

                case CefHandlerKey.DialogHandler:
                    return typeof(IDialogHandler);

                case CefHandlerKey.DragHandler:
                    return typeof(IDragHandler);

                case CefHandlerKey.GeolocationHandler:
                    return typeof(IGeolocationHandler);

                case CefHandlerKey.DownloadHandler:
                    return typeof(IDownloadHandler);

                case CefHandlerKey.FindHandler:
                    return typeof(IFindHandler);

            }

            return null;
        }

        public static List<CefHandlerKey> GetAllHandlerKeys()
        {
            List<CefHandlerKey> types = new List<CefHandlerKey>();

            types.Add(CefHandlerKey.LifeSpanHandler);
            types.Add(CefHandlerKey.LoadHandler);
            types.Add(CefHandlerKey.RequestHandler);
            types.Add(CefHandlerKey.DisplayHandler);
            types.Add(CefHandlerKey.ContextMenuHandler);
            types.Add(CefHandlerKey.FocusHandler);
            types.Add(CefHandlerKey.KeyboardHandler);
            types.Add(CefHandlerKey.JSDialogHandler);
            types.Add(CefHandlerKey.DialogHandler);
            types.Add(CefHandlerKey.DragHandler);
            types.Add(CefHandlerKey.GeolocationHandler);
            types.Add(CefHandlerKey.DownloadHandler);
            types.Add(CefHandlerKey.FindHandler);

            return types;
        }
    }
}
