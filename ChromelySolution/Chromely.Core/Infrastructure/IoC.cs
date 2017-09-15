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
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;

    public static class IoC
    {
        static IChromelyContainer m_container = new SimpleContainer();

        public static IChromelyContainer Container
        {
            get { return m_container; }
            set
            {
                m_container = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public static void RegisterInstance(Type service, string key, object implementation)
        {
            if (m_container != null)
            {
                m_container.RegisterInstance(service, key, implementation);
            }
        }

        public static void RegisterPerRequest(Type service, string key, Type implementation)
        {
            if (m_container != null)
            {
                m_container.RegisterPerRequest(service, key, implementation);
            }
        }

        public static void RegisterSingleton(Type service, string key, Type implementation)
        {
            if (m_container != null)
            {
                m_container.RegisterSingleton(service, key, implementation);
            }
        }

        public static void RegisterHandler(Type service, string key, Func<IChromelyContainer, object> handler)
        {
            if (m_container != null)
            {
                m_container.RegisterHandler(service, key, handler);
            }
        }

        public static void UnregisterHandler(Type service, string key)
        {
            if (m_container != null)
            {
                m_container.UnregisterHandler(service, key);
            }
        }

        public static object GetInstance(Type service, string key)
        {
            if (m_container != null)
            {
                return m_container.GetInstance(service, key);
            }

            return null;
        }

        public static bool HasHandler(Type service, string key)
        {
            if (m_container != null)
            {
                return m_container.HasHandler(service, key);
            }

            return false;
        }

        public static IEnumerable<object> GetAllInstances(Type service)
        {
            if (m_container != null)
            {
                return m_container.GetAllInstances(service);
            }

            return null;
        }

        public static void BuildUp(object instance)
        {
            if (m_container != null)
            {
                m_container.BuildUp(instance);
            }
        }
    }
}