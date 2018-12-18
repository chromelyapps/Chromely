// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
// See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefGlue.Gtk.BrowserWindow
{
    using Chromely.Core;
    using Chromely.Core.Host;
    using System;

    /// <summary>
    /// The native window.
    /// </summary>
    public class NativeWindow
    {
        /// <summary>
        /// The event lock object.
        /// </summary>
        private readonly object mEventLock = new object();

        /// <summary>
        /// The m host config.
        /// </summary>
        private readonly ChromelyConfiguration mHostConfig;

        /// <summary>
        /// The main window.
        /// </summary>
        private IntPtr mMainWindow;

        /// <summary>
        /// The realize event.
        /// </summary>
        private EventHandler<EventArgs> mRealizeEvent;

        /// <summary>
        /// The configure event.
        /// </summary>
        private EventHandler<EventArgs> mConfigureEvent;

        /// <summary>
        /// The destroy event.
        /// </summary>
        private EventHandler<EventArgs> mDestroyEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeWindow"/> class.
        /// </summary>
        public NativeWindow()
        {
            this.mMainWindow = IntPtr.Zero;
            this.mHostConfig = ChromelyConfiguration.Create();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeWindow"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public NativeWindow(ChromelyConfiguration hostConfig)
        {
            this.mMainWindow = IntPtr.Zero;
            this.mHostConfig = hostConfig;
        }

        #region Events

        /// <summary>
        /// The realized.
        /// </summary>
        private event EventHandler<EventArgs> Realized
        {
            add
            {
                lock (mEventLock)
                {
                    mRealizeEvent += value;
                    NativeMethods.EventHandler onRealizedHandler = LocalRealized;
                    NativeMethods.ConnectSignal(mMainWindow, "realize", onRealizedHandler, 0, IntPtr.Zero, (int)NativeMethods.GConnectFlags.GConnectAfter);
                }
            }

            remove
            {
                lock (mEventLock)
                {
                    // ReSharper disable once DelegateSubtraction
                    mRealizeEvent -= value;
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
                lock (mEventLock)
                {
                    mConfigureEvent += value;
                    NativeMethods.EventHandler onConfiguredHandler = LocalConfigured;
                    NativeMethods.ConnectSignal(mMainWindow, "configure-event", onConfiguredHandler, 0, IntPtr.Zero, (int)NativeMethods.GConnectFlags.GConnectAfter);
                }
            }

            remove
            {
                lock (mEventLock)
                {
                    // ReSharper disable once DelegateSubtraction
                    mConfigureEvent -= value;
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
                lock (mEventLock)
                {
                    mDestroyEvent += value;
                    NativeMethods.EventHandler onDestroyedHandler = LocalDestroyed;
                    NativeMethods.ConnectSignal(mMainWindow, "destroy", onDestroyedHandler, 0, IntPtr.Zero, (int)NativeMethods.GConnectFlags.GConnectAfter);
                }
            }

            remove
            {
                lock (mEventLock)
                {
                    // ReSharper disable once DelegateSubtraction
                    mDestroyEvent -= value;
                }
            }
        }

        #endregion Events

        /// <summary>
        /// Gets the handle.
        /// </summary>
        public IntPtr Handle => mMainWindow;

        /// <summary>
        /// The host.
        /// </summary>
        public IntPtr Host => mMainWindow;

        /// <summary>
        /// The host xid.
        /// </summary>
        public IntPtr HostXid => NativeMethods.GetWindowXid(mMainWindow);

        /// <summary>
        /// The show window.
        /// </summary>
        public void ShowWindow()
        {
            CreateWindow();
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

            if (mMainWindow == IntPtr.Zero)
            {
                return;
            }

            NativeMethods.GetWindowSize(Host, out width, out height);
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
            GetSize(out var width, out var height);
            NativeMethods.SetWindowSize(Host, width, height);
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
            NativeMethods.SetTitle(this.mMainWindow, this.mHostConfig.HostTitle);

            NativeMethods.SetIconFromFile(this.mMainWindow, this.mHostConfig.HostIconFile);

            NativeMethods.SetSizeRequest(this.mMainWindow, this.mHostConfig.HostWidth, this.mHostConfig.HostHeight);

            if (this.mHostConfig.HostCenterScreen)
            {
                NativeMethods.SetWindowPosition(this.mMainWindow, NativeMethods.GtkWindowPosition.GtkWinPosCenter);
            }

            switch (this.mHostConfig.HostState)
            {
                case WindowState.Normal:
                    break;

                case WindowState.Maximize:
                    NativeMethods.SetWindowMaximize(this.mMainWindow);
                    break;

                case WindowState.Fullscreen:
                    NativeMethods.SetFullscreen(this.mMainWindow);
                    break;
            }

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
            mRealizeEvent(this, new EventArgs());
        }

        /// <summary>
        /// The local configured.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void LocalConfigured(NativeMethods.StructEventArgs eventArgs)
        {
            mConfigureEvent(this, new EventArgs());
        }

        /// <summary>
        /// The local destroyed.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void LocalDestroyed(NativeMethods.StructEventArgs eventArgs)
        {
            mDestroyEvent(this, new EventArgs());
        }
    }
}
