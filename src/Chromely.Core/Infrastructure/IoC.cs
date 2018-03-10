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
    using Caliburn.Light;
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

        public static bool IsRegistered(Type service, string key)
        {
            if (m_container != null)
            {
                return m_container.IsRegistered(service, key);
            }

            return false;
        }

        public static bool IsRegistered<TService>(string key)
        {
            if (m_container != null)
            {
                return m_container.IsRegistered<TService>(key);
            }

            return false;
        }

        public static void RegisterSingleton(Type service, string key, Type implementation)
        {
            if (m_container != null)
            {
                m_container.RegisterSingleton(service, key, implementation);
            }
        }

        public static void RegisterSingleton<TService, TImplementation>(string key) where TImplementation : TService
        {
            if (m_container != null)
            {
                m_container.RegisterSingleton<TService, TImplementation>(key);
            }
        }

        public static void RegisterInstance(Type service, string key, object instance)
        {
            if (m_container != null)
            {
                m_container.RegisterInstance(service, key, instance);
            }
        }

        public static void RegisterInstance<TService>(string key, TService instance)
        {
            if (m_container != null)
            {
                m_container.RegisterInstance<TService>(key, instance);
            }
        }

        public static void RegisterPerRequest(Type service, string key, Type implementation)
        {
            if (m_container != null)
            {
                m_container.RegisterPerRequest(service, key, implementation);
            }
        }

        public static void RegisterPerRequest<TService, TImplementation>(string key) where TImplementation : TService
        {
            if (m_container != null)
            {
                m_container.RegisterPerRequest<TService, TImplementation>(key);
            }
        }

        public static void UnregisterHandler(Type service, string key)
        {
            if (m_container != null)
            {
                m_container.UnregisterHandler(service, key);
            }
        }

        public static void UnregisterHandler<TService>(string key)
        {
            if (m_container != null)
            {
                m_container.UnregisterHandler<TService>(key);
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

        public static TService GetInstance<TService>(string key)
        {
            if (m_container != null)
            {
                return m_container.GetInstance<TService>(key);
            }

            return default(TService);
        }

        public static object[] GetAllInstances(Type service)
        {
            if (m_container != null)
            {
                return m_container.GetAllInstances(service);
            }

            return null;
        }

        public static TService[] GetAllInstances<TService>()
        {
            if (m_container != null)
            {
                return m_container.GetAllInstances<TService>();
            }

            return null;
        }


    }
}