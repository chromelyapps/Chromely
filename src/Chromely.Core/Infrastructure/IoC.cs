// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Caliburn.Light;

// ReSharper disable UnusedMember.Global
namespace Chromely.Core.Infrastructure
{
    /// <summary>
    /// Global IOC container implementation.
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// The default Chromely container.
        /// </summary>
        private static IChromelyContainer _container = new SimpleContainer();

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public static IChromelyContainer Container
        {
            get => _container;
            set => _container = value;
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
            return (_container != null) && _container.IsRegistered(service, key);
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
            return (_container != null) && _container.IsRegistered<TService>(key);
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
            _container?.RegisterSingleton(service, key, implementation);
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
            _container?.RegisterSingleton<TService, TImplementation>(key);
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
            _container?.RegisterInstance(service, key, instance);
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
            _container?.RegisterInstance(key, instance);
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
            _container?.RegisterPerRequest(service, key, implementation);
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
            _container?.RegisterPerRequest<TService, TImplementation>(key);
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
            _container?.UnregisterHandler(service, key);
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
            _container?.UnregisterHandler<TService>(key);
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
            return _container?.GetInstance(service, key);
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
            return (_container != null) ? _container.GetInstance<TService>(key) : default(TService);
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
            return _container?.GetAllInstances(service);
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
            return _container?.GetAllInstances<TService>();
        }
    }
}