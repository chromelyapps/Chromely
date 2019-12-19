// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleLogger.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;

// ReSharper disable InconsistentNaming
namespace Chromely.Core.Logging
{
    internal class LogEntry
    {
        /// <summary>Initializes a new instance of the <see cref="LogEntry" /> class.</summary>
        public LogEntry(LogLevel level, string entry)
        {
            LogLevel = level;
            Entry = entry;
        }

        /// <summary>Initializes a new instance of the <see cref="LogEntry" /> class.</summary>
        public LogEntry(LogLevel level, Exception error)
        {
            LogLevel = level;
            Error = error;
        }

        /// <summary>Initializes a new instance of the <see cref="LogEntry" /> class.</summary>
        public LogEntry(LogLevel level, string entry, Exception error)
        {
            LogLevel = level;
            Entry = entry;
            Error = error;
        }

        /// <summary>
        /// Gets the log level.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Local
        public LogLevel LogLevel { get; }

        /// <summary>
        /// Gets the entry.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Local
        public string Entry { get; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Local
        public Exception Error { get; set; }

        /// <summary>
        /// The to string.
        /// </summary>
        public override string ToString()
        {
            const string DefaultMessage = "Oops! Something went wrong.";
            string formattedMessage = Entry;

            if (string.IsNullOrEmpty(formattedMessage))
            {
                formattedMessage = DefaultMessage;
            }

            if (Error != null)
            {
                formattedMessage = $"{formattedMessage}\t{Error.Message}\t{Error.StackTrace}";
            }

            string levelText;
            switch (LogLevel)
            {
                case LogLevel.TRACE:
                    levelText = "[TRACE]";
                    break;

                case LogLevel.INFO:
                    levelText = "[INFO]";
                    break;

                case LogLevel.VERBOSE:
                    levelText = "[VERBOSE]";
                    break;

                case LogLevel.DEBUG:
                    levelText = "[DEBUG]";
                    break;

                case LogLevel.WARN:
                    levelText = "[WARNING]";
                    break;

                case LogLevel.ERROR:
                    levelText = "[ERROR]";
                    break;

                case LogLevel.FATAL:
                    levelText = "[FATAL]";
                    break;

                case LogLevel.CRITICAL:
                    levelText = "[CRITICAL]";
                    break;

                default:
                    levelText = string.Empty;
                    break;
            }

            string datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            return $"{DateTime.Now.ToString(datetimeFormat)} {levelText} {formattedMessage}";
        }
    }
}