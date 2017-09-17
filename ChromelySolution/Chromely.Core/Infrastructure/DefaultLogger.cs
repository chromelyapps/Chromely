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
    using Serilog;
    using System;

    public class DefaultLogger : IChromelyLogger
    {
        private Serilog.Core.Logger m_logger;

        public DefaultLogger(string rollingFile = null)
        {
            if (string.IsNullOrEmpty(rollingFile))
            {
                rollingFile = "logs\\chromely.log";
            }

            m_logger = new LoggerConfiguration()
             .WriteTo.Console()
             .WriteTo.RollingFile(rollingFile)
             .CreateLogger();
        }

        public void Info(string message)
        {
            m_logger.Information(message);
        }

        public void Debug(string message)
        {
            m_logger.Debug(message);
        }

        public void Verbose(string message)
        {
            m_logger.Verbose(message);
        }

        public void Warn(string message)
        {
            m_logger.Warning(message);
        }

        public void Error(string message)
        {
            m_logger.Error(message);
        }

        public void Error(Exception exception, string message = null)
        {
            message = FromFormattedException(exception, message);
            m_logger.Error(message);
        }

        public void Fatal(string message)
        {
            m_logger.Fatal(message);
        }

        public void Critial(string message)
        {
            m_logger.Fatal(message);
        }

        public static string FromFormattedException(Exception exception, string message = null)
        {
            const string defaultMessage = "Oops! Something went wrong.";
            if (message == null)
            {
                message = defaultMessage;
            }

            string formattedMessage;
            if (exception != null)
            {
                formattedMessage = $"{message}{Environment.NewLine}" +
                      $"{exception.Message}{Environment.NewLine}" +
                      $"{exception.StackTrace}";
            }
            else
            {
                formattedMessage = $"{message}{Environment.NewLine}" +
                      $"{exception}";
            }

            return formattedMessage;
        }
    }
}
