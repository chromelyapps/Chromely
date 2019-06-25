// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Log.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Chromely.Core.Infrastructure
{
    /// <summary>
    /// Global logger implementation.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// The logger object.
        /// </summary>
        private static IChromelyLogger _logger;

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public static IChromelyLogger Logger
        {
            get => _logger ?? (_logger = GetCurrentLogger);
            set => _logger = value;
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