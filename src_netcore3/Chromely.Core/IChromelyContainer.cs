// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChromelyContainer.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Chromely.Core
{
    /// <summary>
    /// The generic Service Locator interface. 
    /// This interface is used to retrieve services (instances identified by type and optional name) from a container.
    /// </summary>
    public interface IChromelyContainer
    {
        /// <summary>
        /// Determines if a handler for the service/key has previously been registered.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <returns>True if a handler is registered; false otherwise.</returns>
        bool IsRegistered(Type service, string key);

        /// <summary>
        /// Determines if a handler for the service/key has previously been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>True if a handler is registered; false otherwise.</returns>
        bool IsRegistered<TService>(string key);

        /// <summary>
        /// Get all keys by service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns>The list of keys.</returns>
        string[] GetKeys(Type service);

        /// <summary>
        /// Registers the class so that it is created once, on first request, and the same instance is returned to all requestors thereafter.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <param name="implementation">The implementation.</param>
        void RegisterSingleton(Type service, string key, Type implementation);

        /// <summary>
        /// Registers the class so that it is created once, on first request, and the same instance is returned to all requestors thereafter.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="key">The key.</param>
        void RegisterSingleton<TImplementation>(string key);

        /// <summary>
        /// Registers the class so that it is created once, on first request, and the same instance is returned to all requestors thereafter.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="key">The key.</param>
        void RegisterSingleton<TService, TImplementation>(string key) where TImplementation : TService;

        /// <summary>
        /// Registers an instance with the container.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <param name="instance">The instance.</param>
        void RegisterInstance(Type service, string key, object instance);

        /// <summary>
        /// Registers an instance with the container.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="instance">The instance.</param>
        void RegisterInstance<TService>(string key, TService instance);

        /// <summary>
        /// Registers the class so that a new instance is created on each request.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <param name="implementation">The implementation.</param>
        void RegisterPerRequest(Type service, string key, Type implementation);

        /// <summary>
        /// Registers the class so that a new instance is created on each request.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        void RegisterPerRequest<TService>(string key);

        /// <summary>
        /// Registers the class so that a new instance is created on each request.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="key">The key.</param>
        void RegisterPerRequest<TService, TImplementation>(string key) where TImplementation : TService;

        /// <summary>
        /// Unregisters any handlers for the service/key that have previously been registered.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <returns>true if handler is successfully removed; otherwise, false.</returns>
        bool UnregisterHandler(Type service, string key);

        /// <summary>
        /// Unregisters any handlers for the service/key that have previously been registered.
        /// </summary>
        /// <typeparam name="TService">The of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>true if handler is successfully removed; otherwise, false.</returns>
        bool UnregisterHandler<TService>(string key);

        /// <summary>
        /// Requests an instance.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <returns>The instance.</returns>
        object GetInstance(Type service, string key);

        /// <summary>
        /// Requests an instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The instance.</returns>
        TService GetInstance<TService>(string key);

        /// <summary>
        /// Requests all instances of a given type.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns>All the instances or an empty enumerable if none are found.</returns>
        object[] GetAllInstances(Type service);

        /// <summary>
        /// Gets all instances of a particular type.
        /// </summary>
        /// <typeparam name="TService">The type to resolve.</typeparam>
        /// <returns>The resolved instances.</returns>
        TService[] GetAllInstances<TService>();
    }
}
