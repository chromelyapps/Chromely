// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Chromely">
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

namespace Chromely.Core.Infrastructure
{
    using System;

    using Caliburn.Light;

    /// <summary>
    /// The io c.
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// The mContainer.
        /// </summary>
        private static IChromelyContainer mContainer = new SimpleContainer();

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public static IChromelyContainer Container
        {
            get => mContainer;
            set => mContainer = value;
        }

        /// <summary>
        /// The is registered.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsRegistered(Type service, string key)
        {
            return (mContainer != null) && mContainer.IsRegistered(service, key);
        }

        /// <summary>
        /// The is registered.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <typeparam name="TService">
        /// Service type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsRegistered<TService>(string key)
        {
            return (mContainer != null) && mContainer.IsRegistered<TService>(key);
        }

        /// <summary>
        /// The register singleton.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="implementation">
        /// The implementation.
        /// </param>
        public static void RegisterSingleton(Type service, string key, Type implementation)
        {
            mContainer?.RegisterSingleton(service, key, implementation);
        }

        /// <summary>
        /// The register singleton.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <typeparam name="TService">
        /// Service type.
        /// </typeparam>
        /// <typeparam name="TImplementation">
        /// Implementation type.
        /// </typeparam>
        public static void RegisterSingleton<TService, TImplementation>(string key) where TImplementation : TService
        {
            mContainer?.RegisterSingleton<TService, TImplementation>(key);
        }

        /// <summary>
        /// The register instance.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        public static void RegisterInstance(Type service, string key, object instance)
        {
            mContainer?.RegisterInstance(service, key, instance);
        }

        /// <summary>
        /// The register instance.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <typeparam name="TService">
        /// Service type.
        /// </typeparam>
        public static void RegisterInstance<TService>(string key, TService instance)
        {
            mContainer?.RegisterInstance(key, instance);
        }

        /// <summary>
        /// The register per request.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="implementation">
        /// The implementation.
        /// </param>
        public static void RegisterPerRequest(Type service, string key, Type implementation)
        {
            mContainer?.RegisterPerRequest(service, key, implementation);
        }

        /// <summary>
        /// The register per request.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <typeparam name="TService">
        /// Service type.
        /// </typeparam>
        /// <typeparam name="TImplementation">
        /// Implementation type.
        /// </typeparam>
        public static void RegisterPerRequest<TService, TImplementation>(string key) where TImplementation : TService
        {
            mContainer?.RegisterPerRequest<TService, TImplementation>(key);
        }

        /// <summary>
        /// The unregister handler.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        public static void UnregisterHandler(Type service, string key)
        {
            mContainer?.UnregisterHandler(service, key);
        }

        /// <summary>
        /// The unregister handler.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <typeparam name="TService">
        /// Service type.
        /// </typeparam>
        public static void UnregisterHandler<TService>(string key)
        {
            mContainer?.UnregisterHandler<TService>(key);
        }

        /// <summary>
        /// The get instance.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object GetInstance(Type service, string key)
        {
            return mContainer?.GetInstance(service, key);
        }

        /// <summary>
        /// The get instance.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <typeparam name="TService">
        /// Service type.
        /// </typeparam>
        /// <returns>
        /// Collection of TService objects to return.
        /// </returns>
        public static TService GetInstance<TService>(string key)
        {
            return (mContainer != null) ? mContainer.GetInstance<TService>(key) : default(TService);
        }

        /// <summary>
        /// The get all instances.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <returns>
        /// Collection of objects to return.
        /// </returns>
        public static object[] GetAllInstances(Type service)
        {
            return mContainer?.GetAllInstances(service);
        }

        /// <summary>
        /// The get all instances.
        /// </summary>
        /// <typeparam name="TService">
        /// Service type.
        /// </typeparam>
        /// <returns>
        /// Collection of TService objects to return.
        /// </returns>
        public static TService[] GetAllInstances<TService>()
        {
            return mContainer?.GetAllInstances<TService>();
        }
    }
}