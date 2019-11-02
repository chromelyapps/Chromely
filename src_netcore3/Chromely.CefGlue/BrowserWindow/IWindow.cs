// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using Chromely.CefGlue.Browser;

namespace Chromely.CefGlue.BrowserWindow
{
    /// <summary>
    /// Interface to a platform specific window.
    /// </summary>
    public interface IWindow : IDisposable
    {
        IntPtr HostHandle { get; }

        CefGlueBrowser Browser { get; }

        void Run();

        void Exit();
    }
}