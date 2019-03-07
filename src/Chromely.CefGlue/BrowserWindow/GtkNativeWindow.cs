// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GtkNativeWindow.cs" company="Chromely Projects">
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
    public class GtkNativeWindow
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
        /// Initializes a new instance of the <see cref="GtkNativeWindow"/> class.
        /// </summary>
        public GtkNativeWindow()
        {
            this.mMainWindow = IntPtr.Zero;
            this.mHostConfig = ChromelyConfiguration.Create();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GtkNativeWindow"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public GtkNativeWindow(ChromelyConfiguration hostConfig)
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
                    GtkNativeMethods.EventHandler onRealizedHandler = LocalRealized;
                    GtkNativeMethods.ConnectSignal(mMainWindow, "realize", onRealizedHandler, 0, IntPtr.Zero, (int)GtkNativeMethods.GConnectFlags.GConnectAfter);
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
                    GtkNativeMethods.EventHandler onConfiguredHandler = LocalConfigured;
                    GtkNativeMethods.ConnectSignal(mMainWindow, "configure-event", onConfiguredHandler, 0, IntPtr.Zero, (int)GtkNativeMethods.GConnectFlags.GConnectAfter);
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
                    GtkNativeMethods.EventHandler onDestroyedHandler = LocalDestroyed;
                    GtkNativeMethods.ConnectSignal(mMainWindow, "destroy", onDestroyedHandler, 0, IntPtr.Zero, (int)GtkNativeMethods.GConnectFlags.GConnectAfter);
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
        public IntPtr HostXid => GtkNativeMethods.GetWindowXid(mMainWindow);

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
            GtkNativeMethods.SetWindowSize(host, width, height);
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

            GtkNativeMethods.GetWindowSize(Host, out width, out height);
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
            GtkNativeMethods.SetWindowSize(Host, width, height);
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
            GtkNativeMethods.Quit();
        }

        /// <summary>
        /// The create window.
        /// </summary>
        private void CreateWindow()
        {
            this.mMainWindow = GtkNativeMethods.NewWindow(GtkNativeMethods.GtkWindowType.GtkWindowToplevel);
            GtkNativeMethods.SetTitle(this.mMainWindow, this.mHostConfig.HostTitle);

            GtkNativeMethods.SetIconFromFile(this.mMainWindow, this.mHostConfig.HostIconFile);

            GtkNativeMethods.SetSizeRequest(this.mMainWindow, this.mHostConfig.HostWidth, this.mHostConfig.HostHeight);

            if (this.mHostConfig.HostCenterScreen)
            {
                GtkNativeMethods.SetWindowPosition(this.mMainWindow, GtkNativeMethods.GtkWindowPosition.GtkWinPosCenter);
            }

            switch (this.mHostConfig.HostState)
            {
                case WindowState.Normal:
                    break;

                case WindowState.Maximize:
                    GtkNativeMethods.SetWindowMaximize(this.mMainWindow);
                    break;

                case WindowState.Fullscreen:
                    GtkNativeMethods.SetFullscreen(this.mMainWindow);
                    break;
            }

            GtkNativeMethods.AddConfigureEvent(this.mMainWindow);

            this.Realized += this.OnRealized;
            this.Resized += this.OnResize;
            this.Exited += this.OnExit;

            GtkNativeMethods.ShowAll(this.mMainWindow);
        }

        /// <summary>
        /// The local realized.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void LocalRealized(GtkNativeMethods.StructEventArgs eventArgs)
        {
            mRealizeEvent(this, new EventArgs());
        }

        /// <summary>
        /// The local configured.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void LocalConfigured(GtkNativeMethods.StructEventArgs eventArgs)
        {
            mConfigureEvent(this, new EventArgs());
        }

        /// <summary>
        /// The local destroyed.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void LocalDestroyed(GtkNativeMethods.StructEventArgs eventArgs)
        {
            mDestroyEvent(this, new EventArgs());
        }
    }
}
