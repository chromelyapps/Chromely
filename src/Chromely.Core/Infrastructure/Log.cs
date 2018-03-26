// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Log.cs" company="Chromely">
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

namespace Chromely.Core.Infrastructure
{
    using System;

    /// <summary>
    /// The log.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// The logger object.
        /// </summary>
        private static IChromelyLogger mLogger;

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public static IChromelyLogger Logger
        {
            get => mLogger ?? (mLogger = GetCurrentLogger);
            set => mLogger = value;
        }

        /// <summary>
        /// Gets the get current logger.
        /// </summary>
        private static IChromelyLogger GetCurrentLogger
        {
            get
            {
                var logger = IoC.GetInstance(typeof(IChromelyLogger), typeof(Log).FullName);
                if (logger is IChromelyLogger chromelyLogger)
                {
                    return chromelyLogger;
                }

                return new SimpleLogger();
            }
        }

        /// <summary>
        /// Logs an info message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Info(string message)
        {
            Logger.Info(message);
        }

        /// <summary>
        /// Logs a debug message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Debug(string message)
        {
            Logger.Debug(message);
        }

        /// <summary>
        /// Logs a verbose message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Verbose(string message)
        {
            Logger.Verbose(message);
        }

        /// <summary>
        /// Logs a warning message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Warn(string message)
        {
            Logger.Warn(message);
        }

        /// <summary>
        /// Logs an error message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Error(string message)
        {
            Logger.Error(message);
        }

        /// <summary>
        /// Logs an error message type.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Error(Exception exception, string message = null)
        {
            Logger.Error(exception, message);
        }

        /// <summary>
        /// Logs a fatal message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Fatal(string message)
        {
            Logger.Fatal(message);
        }

        /// <summary>
        /// Logs a critical message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Critial(string message)
        {
            Logger.Fatal(message);
        }
    }
}