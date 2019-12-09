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
namespace Chromely.Core.Logging
{
    public class SimpleLogger : IChromelyLogger
    {
        private readonly string _filename;

        private readonly int _maxSizeInKiloBytes;

        private readonly bool _logToConsole;

        private readonly object _lockObj = new object();

        /// <summary>Initializes a new instance of the <see cref="SimpleLogger" /> class.</summary>
        /// <param name="fullFilePath">The full file path.</param>
        /// <param name="logToConsole">if set to <c>true</c> log to console.</param>
        /// <param name="maxFileSizeBeforeLogRotation">The maximum file size before log rotation in MB.</param>
        public SimpleLogger(string fullFilePath = null, bool logToConsole = true, int maxFileSizeBeforeLogRotation = 10)
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
            maxFileSizeBeforeLogRotation = (maxFileSizeBeforeLogRotation < -0) ? 10 : maxFileSizeBeforeLogRotation;
            _maxSizeInKiloBytes = 1000 * maxFileSizeBeforeLogRotation; 
        }

        /// <summary>Logs an info message type.</summary>
        public void Info(string message)
        {
            Log(new LogEntry(LogLevel.INFO, message));
        }

        /// <summary>Logs a debug message type.</summary>
        public void Debug(string message)
        {
            Log(new LogEntry(LogLevel.DEBUG, message));
        }

        /// <summary>Logs a verbose message type.</summary>
        /// <param name="message">The message.</param>
        public void Verbose(string message)
        {
            Log(new LogEntry(LogLevel.VERBOSE, message));
        }

        /// <summary>Logs a warning message type.</summary>
        public void Warn(string message)
        {
            Log(new LogEntry(LogLevel.WARN, message));
        }

        /// <summary>Logs an error message type.</summary>
        public void Error(string message)
        {
            Log(new LogEntry(LogLevel.ERROR, message));
        }

        /// <summary>Logs an error message type.</summary>
        public void Error(Exception exception)
        {
            Log(new LogEntry(LogLevel.ERROR, exception));
        }

        /// <summary>Logs an error message type.</summary>
        public void Error(Exception exception, string message)
        {
            Log(new LogEntry(LogLevel.ERROR, message, exception));
        }

        /// <summary>Logs a fatal message type.</summary>
        public void Fatal(string message)
        {
            Log(new LogEntry(LogLevel.FATAL, message));
        }

        /// <summary>Logs a critical message type.</summary>
        public void Critial(string message)
        {
            Log(new LogEntry(LogLevel.CRITICAL, message));
        }

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

        private void WriteToConsole(string text)
        {
            Console.WriteLine(text);
        }

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
    }
}
