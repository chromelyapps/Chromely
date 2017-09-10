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

namespace Chromely.Infrastructure
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Factory to create and maintain known loggers.
    /// </summary>
    public static class LoggerFactory
    {
        private static string ChromelyDefaultLoggerName = Guid.NewGuid().ToString();
        private static string DefaultLoggerName = null;
        private static Dictionary<string, ILogger> Mapping
            = new Dictionary<string, ILogger>();
        private static object m_lockThis = new object();

        public static void RegisterLogger(string name, ILogger logger, bool isDefault = false)
        {
            if (isDefault)
            {
                DefaultLoggerName = name;
            }

            if (!Mapping.ContainsKey(name))
            {
                Mapping.Add(name, logger);
            }
        }

        public static void SetUpLogger(Action action)
        {
            if (action == null)
            {
                action.Invoke();
            }
        }

        public static ILogger GetLogger(string name = null)
        {
            lock (m_lockThis)
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = string.IsNullOrEmpty(DefaultLoggerName) ? ChromelyDefaultLoggerName : DefaultLoggerName;
                    if (!Mapping.ContainsKey(name))
                    {
                        RegisterLogger(name, new ChromelyDefaultLogger());
                    }
                }

                if (Mapping.ContainsKey(name))
                {
                    return Mapping[name];
                }

                return null;
            }
        }
    }
}
