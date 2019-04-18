using System;
using Chromely.CefGlue.Browser;

namespace Chromely.CefGlue.BrowserWindow
{
    public interface IWindow : IDisposable
    {
        CefGlueBrowser Browser { get; }
        void CenterToScreen();
        void Exit();
    }
}