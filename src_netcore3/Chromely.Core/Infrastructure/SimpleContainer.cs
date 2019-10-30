// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleContainer.cs" company="Chromely Projects">
//   Copyright (c) 2017-2019 Chromely Projects
// </copyright>
// <license>
//      See the LICENSE.md file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------

// --------------------------------------------------------------------------------------------------------------------
// This is a port of Caliburn.Light SimpleContainer for Chromely.Mostly provided as-is. 
// For more info: https://github.com/tibel/Caliburn.Light/blob/master/src/Caliburn.Core/IoC/SimpleContainer.cs
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Chromely.Core;

// ReSharper disable once CheckNamespace
// ReSharper disable UnusedMember.Global
namespace Caliburn.Light
{
    /// <summary>
    /// A simple IoC container.
    /// </summary>
    public class SimpleContainer : IChromelyContainer
    {
        /// <summary>
        /// The delegate type.
        /// </summary>
        private static readonly TypeInfo DelegateType = typeof(Delegate).GetTypeInfo();

        /// <summary>
        /// The enumerable type.
        /// </summary>
        private static readonly TypeInfo EnumerableType = typeof(IEnumerable).GetTypeInfo();

        /// <summary>
        /// The list of entries.
        /// </summary>
        private readonly List<ContainerEntry> _entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleContainer" /> class.
        /// </summary>
        public SimpleContainer()
        {
            _entries = new List<ContainerEntry>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleContainer"/> class.
        /// </summary>
        /// <param name="entries">
        /// The entries.
        /// </param>
        private SimpleContainer(IEnumerable<ContainerEntry> entries)
        {
            _entries = new List<ContainerEntry>(entries);
        }

        /// <summary>
        /// Creates a child container.
        /// </summary>
        /// <returns>A new container.</returns>
        public SimpleContainer CreateChildContainer()
        {
            // ReSharper disable once InconsistentlySynchronizedField
            return new SimpleContainer(_entries);
        }

        /// <summary>
        /// Determines if a handler for the service/key has previously been registered.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <returns>True if a handler is registered; false otherwise.</returns>
        public bool IsRegistered(Type service, string key = null)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            lock (_entries)
            {
                return _entries.Any(x => x.Service == service && x.Key == key);
            }
        }

        /// <summary>
        /// Determines if a handler for the service/key has previously been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>True if a handler is registered; false otherwise.</returns>
        public bool IsRegistered<TService>(string key = null)
        {
            return IsRegistered(typeof(TService), key);
        }

        /// <summary>
        /// Get all keys by service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns>The list of keys.</returns>
        public string[] GetKeys(Type service)
        {
            if (service == null)
            {
                return new string[] {};
            }

            lock (_entries)
            {
                var keys = _entries
                    .Where(x => x.Service == service)
                    .Select(e => e.Key)
                    .ToArray();

                return keys;
            }
        }

        /// <summary>
        /// Registers the class so that it is created once, on first request, and the same instance is returned to all requestors thereafter.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <param name="implementation">The implementation.</param>
        public void RegisterSingleton(Type service, string key, Type implementation)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementation == null)
            {
                throw new ArgumentNullException(nameof(implementation));
            }

            object singleton = null;
            GetOrCreateEntry(service, key)?.Add(c => singleton ?? (singleton = c.BuildInstance(implementation)));
        }

        /// <summary>
        /// Registers the class so that it is created once, on first request, and the same instance is returned to all requestors thereafter.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="key">The key.</param>
        public void RegisterSingleton<TImplementation>(string key = null)
        {
            RegisterSingleton(typeof(TImplementation), key, typeof(TImplementation));
        }

        /// <summary>
        /// Registers the class so that it is created once, on first request, and the same instance is returned to all requestors thereafter.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="key">The key.</param>
        public void RegisterSingleton<TService, TImplementation>(string key = null)
            where TImplementation : TService
        {
            RegisterSingleton(typeof(TService), key, typeof(TImplementation));
        }

        /// <summary>
        /// Registers the class so that it is created once, on first request, and the same instance is returned to all requestors thereafter.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <param name="key">The key.</param>
        public void RegisterSingleton<TService>(Func<SimpleContainer, TService> handler, string key = null)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            object singleton = null;
            GetOrCreateEntry(typeof(TService), key)?.Add(c => singleton ?? (singleton = handler(c)));
        }

        /// <summary>
        /// Registers an instance with the container.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <param name="instance">The instance.</param>
        public void RegisterInstance(Type service, string key, object instance)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            GetOrCreateEntry(service, key)?.Add(c => instance);
        }

        /// <summary>
        /// Registers an instance with the container.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="instance">The instance.</param>
        public void RegisterInstance<TService>(string key, TService instance)
        {
            RegisterInstance(typeof(TService), key, instance);
        }

        /// <summary>
        /// Registers the class so that a new instance is created on each request.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <param name="implementation">The implementation.</param>
        public void RegisterPerRequest(Type service, string key, Type implementation)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementation == null)
            {
                throw new ArgumentNullException(nameof(implementation));
            }

            GetOrCreateEntry(service, key)?.Add(c => c.BuildInstance(implementation));
        }

        /// <summary>
        /// Registers the class so that a new instance is created on each request.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        public void RegisterPerRequest<TService>(string key = null)
        {
            RegisterPerRequest<TService, TService>(key);
        }

        /// <summary>
        /// Registers the class so that a new instance is created on each request.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="key">The key.</param>
        public void RegisterPerRequest<TService, TImplementation>(string key = null)
            where TImplementation : TService
        {
            RegisterPerRequest(typeof(TService), key, typeof(TImplementation));
        }

        /// <summary>
        /// Registers a custom handler for serving requests from the container.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="key">The key.</param>
        public void RegisterPerRequest(Type service, Func<SimpleContainer, object> handler, string key = null)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            GetOrCreateEntry(service, key)?.Add(handler);
        }

        /// <summary>
        /// Registers a custom handler for serving requests from the container.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <param name="key">The key.</param>
        public void RegisterPerRequest<TService>(Func<SimpleContainer, TService> handler, string key = null)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            GetOrCreateEntry(typeof(TService), key)?.Add(c => handler(c));
        }

        /// <summary>
        /// Unregisters any handlers for the service/key that have previously been registered.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <returns>true if handler is successfully removed; otherwise, false.</returns>
        public bool UnregisterHandler(Type service, string key = null)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            lock (_entries)
            {
                var entry = _entries.FirstOrDefault(x => x.Service == service && x.Key == key);
                if (entry == null)
                {
                    return false;
                }

                return _entries.Remove(entry);
            }
        }

        /// <summary>
        /// Unregisters any handlers for the service/key that have previously been registered.
        /// </summary>
        /// <typeparam name="TService">The of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>true if handler is successfully removed; otherwise, false.</returns>
        public bool UnregisterHandler<TService>(string key = null)
        {
            return UnregisterHandler(typeof(TService), key);
        }

        /// <summary>
        /// Requests an instance.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <returns>The instance.</returns>
        public object GetInstance(Type service, string key = null)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            lock (_entries)
            {
                var entry = _entries.FirstOrDefault(x => x.Service == service && x.Key == key);
                if (entry != null)
                {
                    if (entry.Count != 1)
                    {
                        throw new InvalidOperationException($"Found multiple registrations for type '{service}' and key {key}.");
                    }

                    return entry[0](this);
                }
            }

            var serviceType = service.GetTypeInfo();

            if (serviceType.IsGenericType && DelegateType.IsAssignableFrom(serviceType))
            {
                var typeToCreate = service.GenericTypeArguments[0];
                var factoryFactoryType = typeof(FactoryFactory<>).MakeGenericType(typeToCreate);
                var factoryFactoryHost = Activator.CreateInstance(factoryFactoryType);
                var factoryFactoryMethod = factoryFactoryType.GetRuntimeMethod("Create", new[] { typeof(SimpleContainer), typeof(string) });
                return factoryFactoryMethod.Invoke(factoryFactoryHost, new object[] { this, key });
            }

            if (serviceType.IsGenericType && EnumerableType.IsAssignableFrom(serviceType))
            {
                if (key != null)
                {
                    throw new InvalidOperationException($"Requesting type '{service}' with key {key} is not supported.");
                }

                var listType = service.GenericTypeArguments[0];
                var instances = GetAllInstances(listType);
                var array = Array.CreateInstance(listType, instances.Length);

                for (var i = 0; i < array.Length; i++)
                {
                    array.SetValue(instances[i], i);
                }

                return array;
            }

            return serviceType.IsValueType ? Activator.CreateInstance(service) : null;
        }

        /// <summary>
        /// Requests an instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The instance.</returns>
        public TService GetInstance<TService>(string key = null)
        {
            return (TService)GetInstance(typeof(TService), key);
        }

        /// <summary>
        /// Requests all instances of a given type.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns>All the instances or an empty enumerable if none are found.</returns>
        public object[] GetAllInstances(Type service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            lock (_entries)
            {
                var instances = _entries
                    .Where(x => x.Service == service)
                    .SelectMany(e => e.Select(x => x(this)))
                    .ToArray();

                return instances;
            }
        }

        /// <summary>
        /// Gets all instances of a particular type.
        /// </summary>
        /// <typeparam name="TService">The type to resolve.</typeparam>
        /// <returns>The resolved instances.</returns>
        public TService[] GetAllInstances<TService>()
        {
            var service = typeof(TService);

            lock (_entries)
            {
                var instances = _entries
                    .Where(x => x.Service == service)
                    .SelectMany(e => e.Select(x => (TService)x(this)))
                    .ToArray();

                return instances;
            }
        }

        /// <summary>
        /// Actually does the work of creating the instance and satisfying it's constructor dependencies.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The build instance.</returns>
        protected object BuildInstance(Type type)
        {
            var constructor = type.GetTypeInfo().DeclaredConstructors
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault(c => c.IsPublic);

            if (constructor == null)
            {
                throw new InvalidOperationException($"Type '{type}' has no public constructor.");
            }

            var args = constructor.GetParameters()
                .Select(info => GetAllInstances(info.ParameterType).FirstOrDefault())
                .ToArray();

            return ActivateInstance(type, args);
        }

        /// <summary>
        /// Creates an instance of the type with the specified constructor arguments.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="args">The constructor args.</param>
        /// <returns>The created instance.</returns>
        protected virtual object ActivateInstance(Type type, object[] args)
        {
            return (args.Length > 0) ? Activator.CreateInstance(type, args) : Activator.CreateInstance(type);
        }

        /// <summary>
        /// The get or create entry.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="ContainerEntry"/>.
        /// </returns>
        private ContainerEntry GetOrCreateEntry(Type service, string key)
        {
            lock (_entries)
            {
                var entry = _entries.FirstOrDefault(x => x.Service == service && x.Key == key);
                if (entry == null)
                {
                    entry = new ContainerEntry { Service = service, Key = key };
                    _entries.Add(entry);
                    return entry;
                }
            }

            return null;
        }

        /// <summary>
        /// The container entry.
        /// </summary>
        private class ContainerEntry : List<Func<SimpleContainer, object>>
        {
            /// <summary>
            /// Gets or sets the key.
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// Gets or sets the service.
            /// </summary>
            public Type Service { get; set; }
        }

        /// <summary>
        /// The factory factory.
        /// </summary>
        /// <typeparam name="T">
        /// Object type.
        /// </typeparam>
        private class FactoryFactory<T>
        {
            /// <summary>
            /// The create.
            /// </summary>
            /// <param name="container">
            /// The container.
            /// </param>
            /// <param name="key">
            /// The key.
            /// </param>
            /// <returns>
            /// Function pointer.
            /// </returns>
            // ReSharper disable once UnusedMember.Local
            public Func<T> Create(SimpleContainer container, string key) => () => (T)container.GetInstance(typeof(T), key);
        }
    }
}
