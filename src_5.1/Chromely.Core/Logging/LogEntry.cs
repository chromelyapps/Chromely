// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Microsoft.Extensions.Logging;
using System;

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
                case LogLevel.Trace:
                    levelText = "[TRACE]";
                    break;

                case LogLevel.Information:
                    levelText = "[INFO]";
                    break;

                case LogLevel.Debug:
                    levelText = "[DEBUG]";
                    break;

                case LogLevel.Warning:
                    levelText = "[WARNING]";
                    break;

                case LogLevel.Error:
                    levelText = "[ERROR]";
                    break;

                default:
                    levelText = "[UNKNOWN LEVEL]";
                    break;
            }

            string datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            return $"{DateTime.Now.ToString(datetimeFormat)} {levelText} {formattedMessage}";
        }
    }
}