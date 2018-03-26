// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeWindow.cs" company="Chromely">
//   Copyright (c) 2017-2018 Kola Oyewumi
// </copyright>
// <license>
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// Chromely project is licensed under MIT License. CefGlue, CefSharp, Winapi may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.ChromeHost
{
    using System;

    /// <summary>
    /// The native window.
    /// </summary>
    public class NativeWindow
    {
        /// <summary>
        /// The m event lock.
        /// </summary>
        private readonly object mEventLock = new object();

        /// <summary>
        /// The m title.
        /// </summary>
        private readonly string mTitle;

        /// <summary>
        /// The m icon file.
        /// </summary>
        private readonly string mIconFile;

        /// <summary>
        /// The m main window.
        /// </summary>
        private IntPtr mMainWindow;

        /// <summary>
        /// The m width.
        /// </summary>
        private int mWidth;

        /// <summary>
        /// The m height.
        /// </summary>
        private int mHeight;

        /// <summary>
        /// The m realize event.
        /// </summary>
        private EventHandler<EventArgs> mRealizeEvent;

        /// <summary>
        /// The m configure event.
        /// </summary>
        private EventHandler<EventArgs> mConfigureEvent;

        /// <summary>
        /// The m destroy event.
        /// </summary>
        private EventHandler<EventArgs> mDestroyEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeWindow"/> class.
        /// </summary>
        public NativeWindow()
        {
            this.mTitle = "chromely";
            this.mIconFile = null;
            this.mWidth = 1200;
            this.mHeight = 800;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeWindow"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="iconFile">
        /// The icon file.
        /// </param>
        public NativeWindow(string title, int width, int height, string iconFile = null)
        {
            this.mTitle = title;
            this.mWidth = width;
            this.mHeight = height;
            this.mIconFile = iconFile;
        }

        #region Events

        /// <summary>
        /// The realized.
        /// </summary>
        private event EventHandler<EventArgs> Realized
        {
            add
            {
                lock (this.mEventLock)
                {
                    this.mRealizeEvent += value;
                    NativeMethods.EventHandler onRealizedHandler = this.LocalRealized;
                    NativeMethods.ConnectSignal(this.mMainWindow, "realize", onRealizedHandler, 0, IntPtr.Zero, (int)NativeMethods.GConnectFlags.GConnectAfter);
                }
            }

            remove
            {
                lock (this.mEventLock)
                {
                    // ReSharper disable once DelegateSubtraction
                    this.mRealizeEvent -= value;
                }
            }
        }

        /// <summary>
        /// The resized.
        /// </summary>
        private event EventHandler<EventArgs> Resized
        {
            add
            {
                lock (this.mEventLock)
                {
                    this.mConfigureEvent += value;
                    NativeMethods.EventHandler onConfiguredHandler = this.LocalConfigured;
                    NativeMethods.ConnectSignal(this.mMainWindow, "configure-event", onConfiguredHandler, 0, IntPtr.Zero, (int)NativeMethods.GConnectFlags.GConnectAfter);
                }
            }

            remove
            {
                lock (this.mEventLock)
                {
                    // ReSharper disable once DelegateSubtraction
                    this.mConfigureEvent -= value;
                }
            }
        }

        /// <summary>
        /// The exited.
        /// </summary>
        private event EventHandler<EventArgs> Exited
        {
            add
            {
                lock (this.mEventLock)
                {
                    this.mDestroyEvent += value;
                    NativeMethods.EventHandler onDestroyedHandler = this.LocalDestroyed;
                    NativeMethods.ConnectSignal(this.mMainWindow, "destroy", onDestroyedHandler, 0, IntPtr.Zero, (int)NativeMethods.GConnectFlags.GConnectAfter);
                }
            }

            remove
            {
                lock (this.mEventLock)
                {
                    // ReSharper disable once DelegateSubtraction
                    this.mDestroyEvent -= value;
                }
            }
        }

        #endregion Events

        /// <summary>
        /// Gets the handle.
        /// </summary>
        public IntPtr Handle => this.mMainWindow;

        /// <summary>
        /// The host.
        /// </summary>
        public IntPtr Host => this.mMainWindow;

        /// <summary>
        /// The host xid.
        /// </summary>
        public IntPtr HostXid => NativeMethods.GetWindowXid(this.mMainWindow);

        /// <summary>
        /// The show window.
        /// </summary>
        public void ShowWindow()
        {
            this.CreateWindow();
        }

        /// <summary>
        /// The resize host.
        /// </summary>
        /// <param name="host">
        /// The host.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public virtual void ResizeHost(IntPtr host, int width, int height)
        {
            NativeMethods.SetWindowSize(host, width, height);
        }

        /// <summary>
        /// The get size.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        protected virtual void GetSize(out int width, out int height)
        {
            width = 0;
            height = 0;

            if (this.mMainWindow == IntPtr.Zero)
            {
                return;
            }

            NativeMethods.GetWindowSize(this.Host, out width, out height);
        }

        /// <summary>
        /// The on realized.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnRealized(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The on resize.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnResize(object sender, EventArgs e)
        {
            this.GetSize(out var width, out var height);
            NativeMethods.SetWindowSize(this.Host, width, height);
        }

        /// <summary>
        /// The on exit.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnExit(object sender, EventArgs e)
        {
            Application.Quit();
        }

        /// <summary>
        /// The create window.
        /// </summary>
        private void CreateWindow()
        {
            this.mMainWindow = NativeMethods.NewWindow(NativeMethods.GtkWindowType.GtkWindowToplevel);
            NativeMethods.SetTitle(this.mMainWindow, this.mTitle);

            NativeMethods.SetIconFromFile(this.mMainWindow, this.mIconFile);

            NativeMethods.SetSizeRequest(this.mMainWindow, this.mWidth, this.mHeight);
            NativeMethods.SetWindowPosition(this.mMainWindow, NativeMethods.GtkWindowPosition.GtkWinPosCenter);

            NativeMethods.AddConfigureEvent(this.mMainWindow);

            this.Realized += this.OnRealized;
            this.Resized += this.OnResize;
            this.Exited += this.OnExit;

            NativeMethods.ShowAll(this.mMainWindow);
        }

        /// <summary>
        /// The local realized.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void LocalRealized(NativeMethods.StructEventArgs eventArgs)
        {
            this.mRealizeEvent(this, new EventArgs());
        }

        /// <summary>
        /// The local configured.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void LocalConfigured(NativeMethods.StructEventArgs eventArgs)
        {
            this.mConfigureEvent(this, new EventArgs());
        }

        /// <summary>
        /// The local destroyed.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void LocalDestroyed(NativeMethods.StructEventArgs eventArgs)
        {
            this.mDestroyEvent(this, new EventArgs());
        }
    }
}
