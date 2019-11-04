// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFramelessOptions.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.Host
{
    public interface IFramelessOptions
    {
        /// <summary>
        /// Gets or sets the height of the drag region.
        /// This is the usual title bar height.
        /// Normal height is 28 device units (pixels) common for windows.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Gets or sets the no drag width of the drag window (right-side buttons).
        /// This is the total width of the title bar buttons (Min, Max, Close, Help).
        /// Normal width is 140 device units (pixels) common for windows.
        /// </summary>
        int NoDragWidth { get; set; }

        /// <summary>
        /// Gets or sets the white stripe height stripping due to WS_SIZEBOX option.
        /// https://stackoverflow.com/questions/39731497/create-window-without-titlebar-with-resizable-border-and-without-bogus-6px-whit
        /// </summary>
        int WhiteStripeHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the frame is draggable.
        /// </summary>
        bool IsDraggable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether frame is resizable.
        /// </summary>
        bool IsResizable { get; set; }
    }
}
