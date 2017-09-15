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

    public static class Log
    {
        static IChromelyLogger m_logger = new DefaultLogger();

        public static IChromelyLogger Logger
        {
            get { return m_logger; }
            set
            {
                m_logger = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public static void Info(string message)
        {
            m_logger.Info(message);
        }

        public static void Debug(string message)
        {
            m_logger.Debug(message);
        }

        public static void Verbose(string message)
        {
            m_logger.Verbose(message);
        }

        public static void Warn(string message)
        {
            m_logger.Warn(message);
        }

        public static void Error(string message)
        {
            m_logger.Error(message);
        }

        public static void Error(Exception exception, string message = null)
        {
            m_logger.Error(exception, message);
        }

        public static void Fatal(string message)
        {
            m_logger.Fatal(message);
        }

        public static void Critial(string message)
        {
            m_logger.Fatal(message);
        }
    }
}