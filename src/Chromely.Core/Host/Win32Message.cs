// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Win32Message.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.Core.Host
{
    using System;
    using System.Drawing;

    /// <summary>
    /// The win 32 message.
    /// </summary>
    public class Win32Message
    {
        /// <summary>
        /// Gets or sets the hwnd.
        /// </summary>
        public IntPtr Hwnd { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public uint Value { get; set; }

        /// <summary>
        /// Gets or sets the w param.
        /// </summary>
        public IntPtr WParam { get; set; }

        /// <summary>
        /// Gets or sets the l param.
        /// </summary>
        public IntPtr LParam { get; set; }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        public uint Time { get; set; }

        /// <summary>
        /// Gets or sets the point.
        /// </summary>
        public Point Point { get; set; }
    }
}
