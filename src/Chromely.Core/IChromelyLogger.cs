// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChromelyLogger.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Chromely.Core
{
    /// <summary>
    /// The ChromelyLogger interface.
    /// </summary>
    public interface IChromelyLogger
    {
        /// <summary>
        /// The info.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void Info(string message);

        /// <summary>
        /// The verbose.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void Verbose(string message);

        /// <summary>
        /// The debug.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void Debug(string message);

        /// <summary>
        /// The warn.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void Warn(string message);

        /// <summary>
        /// The critial.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void Critial(string message);

        /// <summary>
        /// The fatal.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void Fatal(string message);

        /// <summary>
        /// The error.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void Error(string message);

        /// <summary>
        /// The error.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        void Error(Exception exception, string message = null);
    }
}