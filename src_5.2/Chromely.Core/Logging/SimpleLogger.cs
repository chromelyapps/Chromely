// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Chromely.Core.Logging
{
    public class SimpleLogger : ILogger
    {
        private readonly string _location;
        private readonly string _filename;
        private readonly string _backupFilename;
        private readonly int _maxSizeInKiloBytes;

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
                var appName = Assembly.GetEntryAssembly()?.GetName().Name;
                appName = string.IsNullOrWhiteSpace(appName) ? Guid.NewGuid().ToString() : appName;
                _backupFilename = appName;
                var fileName = $"{appName}.log";
                _location = Path.Combine(exeLocation, "Logs");
                fullFilePath = Path.Combine(_location, fileName);
            }
            else
            {
                _backupFilename = Path.GetFileNameWithoutExtension(fullFilePath);
                _location = Path.GetDirectoryName(fullFilePath);
            }

            _filename = fullFilePath;

            // 10 MB Max size before creating backup - not set
            maxFileSizeBeforeLogRotation = (maxFileSizeBeforeLogRotation < -0) ? 10 : maxFileSizeBeforeLogRotation;
            _maxSizeInKiloBytes = 1000 * maxFileSizeBeforeLogRotation; 
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var builder = new StringBuilder();
            if (formatter != null)
            {
                builder.Append(formatter(state, exception));
            }

            if (exception != null)
            {
                builder.Append(" ");
                builder.Append(exception.ToString());
            }

            Log(new LogEntry(logLevel, builder.ToString()));
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        private void Log(LogEntry entry)
        {
            lock (_lockObj)
            {
                try
                {
                    if (entry != null)
                    {
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
            if (fileInfo.Exists && ((fileInfo.Length / 1024) >= _maxSizeInKiloBytes))
            {
                CreateCopyOfCurrentLogFile(_filename);
            }

            var writer = new StreamWriter(_filename, true, Encoding.UTF8) { AutoFlush = true };
            writer.WriteLine(text);
            writer.Close();
            writer.Dispose();
        }

        private void CreateCopyOfCurrentLogFile(string filePath)
        {
            try
            {
                for (var i = 1; i < 999; i++)
                {
                    var backupPath = Path.Combine(_location, "backup");
                    if (!Directory.Exists(backupPath))
                    {
                        Directory.CreateDirectory(backupPath);
                    }

                    var backupFile = $"{_backupFilename}_{DateTime.Now.ToString("yyyyMMdd")}_{i}.backup";
                    var possibleFilePath = Path.Combine(backupPath, backupFile);
                    if (!File.Exists(possibleFilePath))
                    {
                        File.Move(filePath, possibleFilePath);
                        break;
                    }
                }
            }
            catch {}
        }
    }
}
