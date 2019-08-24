// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowPlacement.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System.Drawing;

namespace Chromely.Core.Host
{
    public class WindowPlacement
    {
        public WindowPlacement()
        {
            Left = 0;
            Top = 0;
            Width = 800;
            Height = 600;
            CenterScreen = true;
            NoResize = false;
            NoMinMaxBoxes = false;
            Frameless = false;
            KioskMode = false;
            State = WindowState.Normal;
            FramelessOptions = new DefaultFramelessOptions();
        }

        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool NoResize { get; set; }
        public bool NoMinMaxBoxes { get; set; }
        public bool Frameless { get; set; }
        public bool CenterScreen { get; set; }
        public bool KioskMode { get; set; }
        public IFramelessOptions FramelessOptions { get; set; }

        /// <summary>
        /// Gets or sets the host state.
        /// </summary>
        public WindowState State { get; set; }

        public Size WindowSize
        {
            get
            {
                return new Size(Width, Height);
            }
        }
    }

    public class DefaultFramelessOptions : IFramelessOptions
    {
        private const int WHITESTRIPEHEIGHT = 8;

        public DefaultFramelessOptions()
        {
            var dragRegion = new FramelessDragRegion();
            Height = dragRegion.Height;
            NoDragWidth = dragRegion.NoDragWidth;
            WhiteStripeHeight = WHITESTRIPEHEIGHT;
            IsDraggable = true;
            IsResizable = true;
        }

        public int Height { get; set; }
        public int NoDragWidth { get; set; }
        public int WhiteStripeHeight { get; set; }
        public bool IsDraggable { get; set; }
        public bool IsResizable { get; set; }
    }
}
