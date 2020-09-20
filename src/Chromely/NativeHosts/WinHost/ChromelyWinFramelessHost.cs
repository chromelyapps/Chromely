// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Runtime.InteropServices;
using Chromely.Core.Configuration;
using static Chromely.Interop.User32;
using Chromely.Core.Host;

namespace Chromely.NativeHost
{
    internal class ChromelyWinFramelessHost : NativeHostBase
    {
        protected DwmFramelessController _dwmFramelessController;
        protected FramelessInfo _framelessInfo;
        protected FramelessOption _framelessOption;

        private bool _disposed;

        public ChromelyWinFramelessHost()
        {
            _framelessInfo = new FramelessInfo(IntPtr.Zero);
            _framelessOption = new FramelessOption();
        }

        protected override void OnCreated(IntPtr hWnd)
        {
            base.OnCreated(hWnd);

            if (_options.Fullscreen || _options.KioskMode)
            {
                throw new NotSupportedException("Fullscreen/Kiok mode is not supported in ChromelyFramelessApp. Please use ChromelyBasicApp instead.");
            }

            _options.WindowFrameless = true;
            _framelessInfo = new FramelessInfo(hWnd);
            _framelessOption = _options.FramelessOption;
            _dwmFramelessController = new DwmFramelessController(_framelessInfo, _framelessOption, HandleSizeChanged);

            _dwmFramelessController.HandleCompositionchanged();
            _dwmFramelessController.HandleThemechanged();
        }

        protected override WindowStylePlacement GetWindowStylePlacement(WindowState state)
        {
            WindowStylePlacement windowStyle = new WindowStylePlacement(_options);
            if (_options.UseCustomStyle && _options != null && _options.CustomStyle.IsValid())
            {
                return GetWindowStyles(_options.CustomStyle, state);
            }

            var styles = WS.OVERLAPPEDWINDOW | WS.CLIPCHILDREN | WS.CLIPSIBLINGS;
            var exStyles = WS_EX.APPWINDOW | WS_EX.WINDOWEDGE | WS_EX.TRANSPARENT;

            windowStyle.Styles = styles;
            windowStyle.ExStyles = exStyles;
            windowStyle.RECT = GetWindowBounds();

            switch (state)
            {
                case WindowState.Normal:
                    windowStyle.ShowCommand = SW.SHOWNORMAL;
                    break;

                case WindowState.Maximize:
                    windowStyle.Styles |= WS.MAXIMIZE;
                    windowStyle.ShowCommand = SW.SHOWMAXIMIZED;
                    break;

                default:
                    windowStyle.ShowCommand = SW.SHOWNORMAL;
                    break;
            }

            return windowStyle;
        }

        public override void ResizeBrowser(IntPtr browserHande, int width, int height)
        {
            var noResize = (_options == null) ? false : _options.DisableResizing;
            var gripSize = (_options?.FramelessOption == null) ? 0 : _options.FramelessOption.ResizeGrip;
            var addMargins = !noResize &&
                             (gripSize > 0) &&
                             (browserHande != IntPtr.Zero) &&
                             (width > (4 * gripSize)) &&
                             (height > (4 * gripSize));

            if (addMargins)
            {
                var left = gripSize;
                var top = gripSize;
                width = width - (2 * gripSize);
                height = height - (2 * gripSize);

                SetWindowPos(browserHande, IntPtr.Zero, left, top, width, height, SWP.NOZORDER);
            }
            else
            {
                SetWindowPos(browserHande, IntPtr.Zero, 0, 0, width, height, SWP.NOZORDER);
            }
        }

        #region Frameless WndProc

        protected override IntPtr WndProc(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam)
        {
            WM wmMsg = (WM)message;
            switch (wmMsg)
            {
                case WM.PARENTNOTIFY:
                    {
                        WM wParmLw = (WM)LOWORD((int)wParam);
                        switch (wParmLw)
                        {
                            case WM.LBUTTONDOWN:
                                _dwmFramelessController?.InitiateWindowDrag(hWnd, lParam);
                                return IntPtr.Zero;

                            default:
                                break;
                        }
                    }
                    break;

                case WM.DWMCOMPOSITIONCHANGED:
                    _dwmFramelessController?.HandleCompositionchanged();
                    return IntPtr.Zero;

                case WM.LBUTTONDOWN:
                    /* Allow window dragging from any point */
                    var handlerNullable = _framelessInfo?.Handle;
                    if (handlerNullable.HasValue)
                    {
                        ReleaseCapture();
                        SendMessageW(handlerNullable.Value, WM.NCLBUTTONDOWN, (IntPtr)HT.CAPTION, IntPtr.Zero);
                    }
                    return IntPtr.Zero;

                case WM.NCACTIVATE:
                    /* DefWindowProc won't repaint the window border if lParam (normally a
                       HRGN) is -1. This is recommended in:
                       https://blogs.msdn.microsoft.com/wpfsdk/2008/09/08/custom-window-chrome-in-wpf/ */
                    return base.WndProc(hWnd, message, wParam, new IntPtr(-1));

                case WM.NCCALCSIZE:
                    _dwmFramelessController?.HandleNCCalcsize(wParam, lParam);
                    return IntPtr.Zero;

                case WM.NCHITTEST:
                    if (_framelessInfo != null)
                    {
                        _framelessInfo.ResizeInMotion = true;
                        var hitTestResultNullable = _dwmFramelessController?.HandleNCHittest(GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam));
                        _framelessInfo.ResizeInMotion = false;
                        return hitTestResultNullable.HasValue ? hitTestResultNullable.Value : IntPtr.Zero;
                    }
                    return IntPtr.Zero;

                case WM.NCPAINT:
                    /* Only block WM_NCPAINT when composition is disabled. If it's blocked
                       when composition is enabled, the window shadow won't be drawn. */
                    var isComposistionEnabedNullable = _framelessInfo?.IsCompositionEnabled;
                    var isComposistionEnabed = isComposistionEnabedNullable.HasValue ? isComposistionEnabedNullable.Value : false;
                    if (!isComposistionEnabed)
                    {
                        return IntPtr.Zero;
                    }
                    break;

                case WM.NCUAHDRAWCAPTION:
                case WM.NCUAHDRAWFRAME:
                    /* These undocumented messages are sent to draw themed window borders.
                       Block them to prevent drawing borders over the client area. */
                    return IntPtr.Zero;

                case WM.SETICON:
                case WM.SETTEXT:
                    /* Disable painting while these messages are handled to prevent them
                       from drawing a window caption over the client area, but only when
                       composition and theming are disabled. These messages don't paint
                       when composition is enabled and blocking WM_NCUAHDRAWCAPTION should
                       be enough to prevent painting when theming is enabled. */
                    isComposistionEnabedNullable = _framelessInfo?.IsCompositionEnabled;
                    isComposistionEnabed = isComposistionEnabedNullable.HasValue ? isComposistionEnabedNullable.Value : false;

                    var isThemeEnabledNullable = _framelessInfo?.IsThemeEnabled;
                    var isThemeEnabledEnabed = isThemeEnabledNullable.HasValue ? isThemeEnabledNullable.Value : false;

                    if (!isComposistionEnabed && !isThemeEnabledEnabed)
                    {
                        var msgResult = _dwmFramelessController?.HandleMessageInvisible(wmMsg, wParam, lParam);
                        return msgResult.HasValue ? msgResult.Value : IntPtr.Zero;
                    }
                    break;

                case WM.THEMECHANGED:
                    _dwmFramelessController?.HandleThemechanged();
                    break;

                case WM.WINDOWPOSCHANGED:
                    WINDOWPOS windPos = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
                    _dwmFramelessController?.HandleWindowPosChanged(windPos);
                    return IntPtr.Zero;

                case WM.CAPTURECHANGED:
                    if (lParam == IntPtr.Zero)
                    {
                        _dwmFramelessController?.ResetDragOperation();
                    }
                    return IntPtr.Zero;

                default:
                    break;
            }

            return base.WndProc(hWnd, message, wParam, lParam);
        }

        #endregion WndProc

        #region OnSizeChanged

        private void HandleSizeChanged(SizeChangedEventArgs arg)
        {
            if (arg != null)
            {
                OnSizeChanged(arg.Width, arg.Height);
            }
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            // If there are managed resources
            if (disposing)
            {
            }

            _dwmFramelessController?.Dispose();

            _disposed = true;

            // Call base class implementation.
            base.Dispose(disposing);
        }
        #endregion
    }
}
