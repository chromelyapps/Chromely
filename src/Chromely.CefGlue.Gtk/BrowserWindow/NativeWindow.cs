// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeWindow.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// ----------------------------------------------------------------------------------------------------------------------

using System;
using Chromely.Core;
using Chromely.Core.Host;

namespace Chromely.CefGlue.Gtk.BrowserWindow
{
    /// <summary>
    /// The native window.
    /// </summary>
    public class NativeWindow
    {
        /// <summary>
        /// The event lock object.
        /// </summary>
        private readonly object _eventLock = new object();

        /// <summary>
        /// The host config.
        /// </summary>
        private readonly ChromelyConfiguration _hostConfig;
        private EventHandler<EventArgs> _realizeEvent;
        private EventHandler<EventArgs> _configureEvent;
        private EventHandler<EventArgs> _destroyEvent;

        private uint hRealizeEvent;
        private uint hConfigureEvent;
        private uint hDestroyEvent;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NativeWindow"/> class.
        /// </summary>
        public NativeWindow()
        {
            Handle = IntPtr.Zero;
            _hostConfig = ChromelyConfiguration.Create();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeWindow"/> class.
        /// </summary>
        /// <param name="hostConfig">
        /// The host config.
        /// </param>
        public NativeWindow(ChromelyConfiguration hostConfig)
        {
            Handle = IntPtr.Zero;
            _hostConfig = hostConfig;
        }

        #region Events

        /// <summary>
        /// The realized.
        /// </summary>
        private event EventHandler<EventArgs> Realized
        {
            add
            {
                lock (_eventLock)
                {
                    _realizeEvent += value;
                    NativeMethods.EventHandler onRealizedHandler = LocalRealized;
                    hRealizeEvent = NativeMethods.ConnectSignal(Handle, "realize", onRealizedHandler, 0, IntPtr.Zero, (int)NativeMethods.GConnectFlags.GConnectAfter);
                }
            }

            remove
            {
                lock (_eventLock)
                {
                    // ReSharper disable once DelegateSubtraction
                    _realizeEvent -= value;
                    NativeMethods.DisconnectSignal(Handle, hRealizeEvent);
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
                lock (_eventLock)
                {
                    _configureEvent += value;
                    NativeMethods.EventHandler onConfiguredHandler = LocalConfigured;
                    hConfigureEvent = NativeMethods.ConnectSignal(Handle, "configure-event", onConfiguredHandler, 0, IntPtr.Zero, (int)NativeMethods.GConnectFlags.GConnectAfter);
                }
            }

            remove
            {
                lock (_eventLock)
                {
                    // ReSharper disable once DelegateSubtraction
                    _configureEvent -= value;
                    NativeMethods.DisconnectSignal(Handle, hConfigureEvent);
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
                lock (_eventLock)
                {
                    _destroyEvent += value;
                    NativeMethods.EventHandler onDestroyedHandler = LocalDestroyed;
                    hDestroyEvent = NativeMethods.ConnectSignal(Handle, "destroy", onDestroyedHandler, 0, IntPtr.Zero, (int)NativeMethods.GConnectFlags.GConnectAfter);
                }
            }

            remove
            {
                lock (_eventLock)
                {
                    // ReSharper disable once DelegateSubtraction
                    _destroyEvent -= value;
                    NativeMethods.DisconnectSignal(Handle, hDestroyEvent);
                }
            }
        }

        #endregion Events

        /// <summary>
        /// Gets the handle.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// The host.
        /// </summary>
        public IntPtr Host => Handle;

        /// <summary>
        /// The host xid.
        /// </summary>
        public IntPtr HostXid => NativeMethods.GetWindowXid(Handle);

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

            if (Handle == IntPtr.Zero)
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
            NativeMethods.Quit();
        }

        /// <summary>
        /// The create window.
        /// </summary>
        private void CreateWindow()
        {
            var wndType = _hostConfig.HostPlacement.Frameless
                ? NativeMethods.GtkWindowType.GtkWindowPopup
                : NativeMethods.GtkWindowType.GtkWindowToplevel;
            
            Handle = NativeMethods.NewWindow(wndType);
            
            NativeMethods.SetTitle(Handle, _hostConfig.HostTitle);
            NativeMethods.SetIconFromFile(Handle, _hostConfig.HostIconFile);
            var placement = _hostConfig.HostPlacement;
            NativeMethods.SetSizeRequest(Handle, placement.Width, placement.Height);

            if (placement.CenterScreen)
            {
                NativeMethods.SetWindowPosition(Handle, NativeMethods.GtkWindowPosition.GtkWinPosCenter);
            }

            switch (_hostConfig.HostPlacement.State)
            {
                case WindowState.Normal:
                    break;

                case WindowState.Maximize:
                    NativeMethods.SetWindowMaximize(Handle);
                    break;

                case WindowState.Fullscreen:
                    NativeMethods.SetFullscreen(Handle);
                    break;
            }

            NativeMethods.AddConfigureEvent(Handle);

            Realized += OnRealized;
            
            // A callback was made on a garbage collected delegate of type 'Chromely.CefGlue.Gtk!Chromely.CefGlue.Gtk.BrowserWindow.NativeMethods+EventHandler::Invoke'.
            // TODO: Find the reason - AND remove handlers at correct time
            if (ChromelyRuntime.Platform != ChromelyPlatform.Linux)
            {
                Resized += OnResize;
            }    

            Exited += OnExit;

            NativeMethods.ShowAll(Handle);
        }

        /// <summary>
        /// The local realized.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void LocalRealized(NativeMethods.StructEventArgs eventArgs)
        {
            _realizeEvent(this, new EventArgs());
        }

        /// <summary>
        /// The local configured.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void LocalConfigured(NativeMethods.StructEventArgs eventArgs)
        {
            _configureEvent(this, new EventArgs());
        }

        /// <summary>
        /// The local destroyed.
        /// </summary>
        /// <param name="eventArgs">
        /// The event args.
        /// </param>
        private void LocalDestroyed(NativeMethods.StructEventArgs eventArgs)
        {
            _destroyEvent(this, new EventArgs());
        }
    }
}
