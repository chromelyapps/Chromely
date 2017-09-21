/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.Core.Infrastructure
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public class SimpleLogger : IChromelyLogger
    {
        private string m_filename;
        private int m_maxSizeInKiloBytes;
        private bool m_logToConsole;
        private object lockObj = new object();

        public SimpleLogger(string fullFilePath = null, bool logToConsole = true)
        {
            if (string.IsNullOrEmpty(fullFilePath))
            {
                string exeLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string appendDay = DateTime.Now.ToString("yyyyMMdd");

                fullFilePath = Path.Combine(exeLocation, "Logs", "chromely_" + appendDay + ".log");
            }

            m_filename = fullFilePath;
            m_logToConsole = logToConsole;

            // 10 MB Max size before creating backup
            m_maxSizeInKiloBytes = 1000 * 10; 
        }

        public void Info(string message)
        {
            Log(new LogEntry(LogLevel.INFO, message));
        }

        public void Debug(string message)
        {
            Log(new LogEntry(LogLevel.DEBUG, message));
        }

        public void Verbose(string message)
        {
            Log(new LogEntry(LogLevel.VERBOSE, message));
        }

        public void Warn(string message)
        {
            Log(new LogEntry(LogLevel.WARN, message));
        }

        public void Error(string message)
        {
            Log(new LogEntry(LogLevel.ERROR, message));
        }

        public void Error(Exception exception, string message = null)
        {
            Log(new LogEntry(LogLevel.ERROR, message, exception));
        }

        public void Fatal(string message)
        {
            Log(new LogEntry(LogLevel.FATAL, message));
        }

        public void Critial(string message)
        {
            Log(new LogEntry(LogLevel.CRITICAL, message));
        }

        private void Log(LogEntry entry)
        {
            lock (lockObj)
            {
                try
                {
                    if (entry != null)
                    {
                        if (m_logToConsole)
                        {
                            WriteToConsole(entry.ToString());
                        }

                        WriteToFile(entry.ToString());
                    }
                }
                catch (Exception)
                {
                    // Swallow
                }
            }
        }

        private void WriteToFile(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var directoryName = Path.GetDirectoryName(m_filename);
            if (!string.IsNullOrWhiteSpace(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            var fileInfo = new FileInfo(m_filename);
            if (fileInfo.Exists && (fileInfo.Length / 1024 >= m_maxSizeInKiloBytes))
            {
                CreateCopyOfCurrentLogFile(m_filename);
            }

            StreamWriter writer = new StreamWriter(m_filename, true, Encoding.UTF8);
            writer.AutoFlush = true;
            writer.WriteLine(text);
            writer.Close();
            writer.Dispose();
            writer = null;
        }

        private void WriteToConsole(string text)
        {
            Console.WriteLine(text);
        }

        private void CreateCopyOfCurrentLogFile(string filePath)
        {
            for (int i = 1; i < 999; i++)
            {
                var possibleFilePath = string.Format("{0}.{1:000}", filePath, i);
                if (!File.Exists(possibleFilePath))
                {
                    File.Move(filePath, possibleFilePath);
                    break;
                }
            }
        }

        private class LogEntry
        {
            public LogLevel LogLevel { get; set; }
            public string Entry { get; set; }
            public Exception Error { get; set; }

            public LogEntry()
            {
                LogLevel = LogLevel.INFO;
                Entry = string.Empty;
                Error = null;
            }

            public LogEntry(LogLevel level, string entry, Exception error = null)
            {
                LogLevel = level;
                Entry = entry;
                Error = error;
            }

            public override string ToString()
            {
                const string defaultMessage = "Oops! Something went wrong.";
                string formattedMessage = Entry;

                if (string.IsNullOrEmpty(formattedMessage))
                {
                    formattedMessage = defaultMessage;
                }

                if (Error != null)
                {
                    formattedMessage = $"{formattedMessage}\t{Error.Message}\t{Error.StackTrace}";
                }

                string levelText;
                switch (LogLevel)
                {
                    case LogLevel.TRACE: levelText = "[TRACE]"; break;
                    case LogLevel.INFO: levelText = "[INFO]"; break;
                    case LogLevel.VERBOSE: levelText = "[VERBOSE]"; break;
                    case LogLevel.DEBUG: levelText = "[DEBUG]"; break;
                    case LogLevel.WARN: levelText = "[WARNING]"; break;
                    case LogLevel.ERROR: levelText = "[ERROR]"; break;
                    case LogLevel.FATAL: levelText = "[FATAL]"; break;
                    case LogLevel.CRITICAL: levelText = "[CRITICAL]"; break;
                    default: levelText = ""; break;
                }

                string datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
                return string.Format("{0} {1} {2}", DateTime.Now.ToString(datetimeFormat), levelText, formattedMessage);
            }
        }

        [Flags]
        private enum LogLevel
        {
            TRACE,
            INFO,
            VERBOSE,
            DEBUG,
            WARN,
            ERROR,
            FATAL,
            CRITICAL
        }
    }
}
