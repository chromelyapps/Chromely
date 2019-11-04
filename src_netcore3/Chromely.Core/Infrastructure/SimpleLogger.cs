// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleLogger.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

// ReSharper disable InconsistentNaming
namespace Chromely.Core.Infrastructure
{
    /// <summary>
    /// The simple logger.
    /// </summary>
    public class SimpleLogger : IChromelyLogger
    {
        /// <summary>
        /// The filename.
        /// </summary>
        private readonly string _filename;

        /// <summary>
        /// The max size in kilo bytes.
        /// </summary>
        private readonly int _maxSizeInKiloBytes;

        /// <summary>
        /// The log to console flag.
        /// </summary>
        private readonly bool _logToConsole;

        /// <summary>
        /// The lock obj.
        /// </summary>
        private readonly object _lockObj = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleLogger"/> class.
        /// </summary>
        /// <param name="fullFilePath">
        /// The full file path.
        /// </param>
        /// <param name="logToConsole">
        /// The log to console.
        /// </param>
        /// <param name="rollingMaxMbFileSize">
        /// The rolling max mb file size.
        /// </param>
        public SimpleLogger(string fullFilePath = null, bool logToConsole = true, int rollingMaxMbFileSize = 10)
        {
            if (string.IsNullOrEmpty(fullFilePath))
            {
                var exeLocation = AppDomain.CurrentDomain.BaseDirectory;
                var fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                fullFilePath = Path.Combine(exeLocation, "Logs", "chromely_" + fileName);
            }

            _filename = fullFilePath;
            _logToConsole = logToConsole;

            // 10 MB Max size before creating backup - not set
            rollingMaxMbFileSize = (rollingMaxMbFileSize < -0) ? 10 : rollingMaxMbFileSize;
            _maxSizeInKiloBytes = 1000 * rollingMaxMbFileSize; 
        }

        /// <summary>
        /// Logs an info message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Info(string message)
        {
            Log(new LogEntry(LogLevel.INFO, message));
        }

        /// <summary>
        /// Logs a debug message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Debug(string message)
        {
            Log(new LogEntry(LogLevel.DEBUG, message));
        }

        /// <summary>
        /// Logs a verbose message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Verbose(string message)
        {
            Log(new LogEntry(LogLevel.VERBOSE, message));
        }

        /// <summary>
        /// Logs a warning message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Warn(string message)
        {
            Log(new LogEntry(LogLevel.WARN, message));
        }

        /// <summary>
        /// Logs an error message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Error(string message)
        {
            Log(new LogEntry(LogLevel.ERROR, message));
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
        public void Error(Exception exception, string message = null)
        {
            Log(new LogEntry(LogLevel.ERROR, message, exception));
        }

        /// <summary>
        /// Logs a fatal message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Fatal(string message)
        {
            Log(new LogEntry(LogLevel.FATAL, message));
        }

        /// <summary>
        /// Logs a critical message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Critial(string message)
        {
            Log(new LogEntry(LogLevel.CRITICAL, message));
        }

        /// <summary>
        /// Logs an entry object.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        private void Log(LogEntry entry)
        {
            lock (_lockObj)
            {
                try
                {
                    if (entry != null)
                    {
                        if (_logToConsole)
                        {
                            WriteToConsole(entry.ToString());
                        }

                        WriteToFile(entry.ToString());
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        private void WriteToFile(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var directoryName = Path.GetDirectoryName(_filename);
            if (!string.IsNullOrWhiteSpace(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            var fileInfo = new FileInfo(_filename);
            if (fileInfo.Exists && (fileInfo.Length / 1024 >= _maxSizeInKiloBytes))
            {
                CreateCopyOfCurrentLogFile(_filename);
            }

            var writer = new StreamWriter(_filename, true, Encoding.UTF8) { AutoFlush = true };
            writer.WriteLine(text);
            writer.Close();
            writer.Dispose();
        }

        /// <summary>
        /// Writes to console.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        private void WriteToConsole(string text)
        {
            Console.WriteLine(text);
        }

        /// <summary>
        /// Creates copy of current log file.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        private void CreateCopyOfCurrentLogFile(string filePath)
        {
            for (var i = 1; i < 999; i++)
            {
                var possibleFilePath = $"{filePath}.{i:000}";
                if (!File.Exists(possibleFilePath))
                {
                    File.Move(filePath, possibleFilePath);
                    break;
                }
            }
        }

        /// <summary>
        /// The log entry.
        /// </summary>
        private class LogEntry
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="LogEntry"/> class.
            /// </summary>
            /// <param name="level">
            /// The level.
            /// </param>
            /// <param name="entry">
            /// The entry.
            /// </param>
            /// <param name="error">
            /// The error.
            /// </param>
            public LogEntry(LogLevel level, string entry, Exception error = null)
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
            /// <returns>
            /// The <see cref="string"/>.
            /// </returns>
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

        /// <summary>
        /// The log level.
        /// </summary>
        [Flags]

        // ReSharper disable once StyleCop.SA1201
        private enum LogLevel
        {
            /// <summary>
            /// The trace.
            /// </summary>
            TRACE,

            /// <summary>
            /// The info.
            /// </summary>
            INFO,

            /// <summary>
            /// The verbose.
            /// </summary>
            VERBOSE,

            /// <summary>
            /// The debug.
            /// </summary>
            DEBUG,

            /// <summary>
            /// The warn.
            /// </summary>
            WARN,

            /// <summary>
            /// The error.
            /// </summary>
            ERROR,

            /// <summary>
            /// The fatal.
            /// </summary>
            FATAL,

            /// <summary>
            /// The critical.
            /// </summary>
            CRITICAL
        }
    }
}
