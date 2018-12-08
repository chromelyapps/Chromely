// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntPtrExtensions.cs" company="Chromely Projects">
//   Copyright (c) 2017-2018 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace Chromely.CefSharp.Winapi.Browser.Internals
{
    using System;

    /// <summary>
    /// The int ptr extensions.
    /// </summary>
    internal static class IntPtrExtensions
    {
        /// <summary>
        /// Do an unchecked conversion from IntPtr to int
        /// so overflow exceptions don't get thrown.
        /// </summary>
        /// <param name="intPtr">the IntPtr to cast</param>
        /// <returns>a 32-bit signed integer</returns>
        public static int CastToInt32(this IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }
    }
}