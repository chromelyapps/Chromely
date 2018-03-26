// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleLogger.cs" company="Chromely">
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

// ReSharper disable InconsistentNaming
namespace Chromely.Core.Infrastructure
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// The simple logger.
    /// </summary>
    public class SimpleLogger : IChromelyLogger
    {
        /// <summary>
        /// The m filename.
        /// </summary>
        private readonly string mFilename;

        /// <summary>
        /// The m max size in kilo bytes.
        /// </summary>
        private readonly int mMaxSizeInKiloBytes;

        /// <summary>
        /// The m log to console.
        /// </summary>
        private readonly bool mLogToConsole;

        /// <summary>
        /// The lock obj.
        /// </summary>
        private readonly object mlockObj = new object();

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
                string exeLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string appendDay = DateTime.Now.ToString("yyyyMMdd");

                fullFilePath = Path.Combine(exeLocation, "Logs", "chromely_" + appendDay + ".log");
            }

            this.mFilename = fullFilePath;
            this.mLogToConsole = logToConsole;

            // 10 MB Max size before creating backup - not set
            rollingMaxMbFileSize = (rollingMaxMbFileSize < -0) ? 10 : rollingMaxMbFileSize;
            this.mMaxSizeInKiloBytes = 1000 * rollingMaxMbFileSize; 
        }

        /// <summary>
        /// Logs an info message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Info(string message)
        {
            this.Log(new LogEntry(LogLevel.INFO, message));
        }

        /// <summary>
        /// Logs a debug message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Debug(string message)
        {
            this.Log(new LogEntry(LogLevel.DEBUG, message));
        }

        /// <summary>
        /// Logs a verbose message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Verbose(string message)
        {
            this.Log(new LogEntry(LogLevel.VERBOSE, message));
        }

        /// <summary>
        /// Logs a warning message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Warn(string message)
        {
            this.Log(new LogEntry(LogLevel.WARN, message));
        }

        /// <summary>
        /// Logs an error message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Error(string message)
        {
            this.Log(new LogEntry(LogLevel.ERROR, message));
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
            this.Log(new LogEntry(LogLevel.ERROR, message, exception));
        }

        /// <summary>
        /// Logs a fatal message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Fatal(string message)
        {
            this.Log(new LogEntry(LogLevel.FATAL, message));
        }

        /// <summary>
        /// Logs a critical message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Critial(string message)
        {
            this.Log(new LogEntry(LogLevel.CRITICAL, message));
        }

        /// <summary>
        /// Logs an entry object.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        private void Log(LogEntry entry)
        {
            lock (this.mlockObj)
            {
                try
                {
                    if (entry != null)
                    {
                        if (this.mLogToConsole)
                        {
                            this.WriteToConsole(entry.ToString());
                        }

                        this.WriteToFile(entry.ToString());
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

            var directoryName = Path.GetDirectoryName(this.mFilename);
            if (!string.IsNullOrWhiteSpace(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            var fileInfo = new FileInfo(this.mFilename);
            if (fileInfo.Exists && (fileInfo.Length / 1024 >= this.mMaxSizeInKiloBytes))
            {
                this.CreateCopyOfCurrentLogFile(this.mFilename);
            }

            var writer = new StreamWriter(this.mFilename, true, Encoding.UTF8) { AutoFlush = true };
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
                var possibleFilePath = string.Format("{0}.{1:000}", filePath, i);
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
                this.LogLevel = level;
                this.Entry = entry;
                this.Error = error;
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
                string formattedMessage = this.Entry;

                if (string.IsNullOrEmpty(formattedMessage))
                {
                    formattedMessage = DefaultMessage;
                }

                if (this.Error != null)
                {
                    formattedMessage = $"{formattedMessage}\t{this.Error.Message}\t{this.Error.StackTrace}";
                }

                string levelText;
                switch (this.LogLevel)
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
                return string.Format("{0} {1} {2}", DateTime.Now.ToString(datetimeFormat), levelText, formattedMessage);
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
