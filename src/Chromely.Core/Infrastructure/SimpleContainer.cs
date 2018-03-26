// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleContainer.cs" company="Chromely">
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
//
// This is a port of Caliburn.Light SimpleContainer for Chromely.Mostly provided as-is. 
// For more info: https://github.com/tibel/Caliburn.Light/blob/master/src/Caliburn.Core/IoC/SimpleContainer.cs
// </note>
// --------------------------------------------------------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace Caliburn.Light
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Chromely.Core;

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
        private readonly List<ContainerEntry> mEntries;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleContainer" /> class.
        /// </summary>
        public SimpleContainer()
        {
            this.mEntries = new List<ContainerEntry>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleContainer"/> class.
        /// </summary>
        /// <param name="entries">
        /// The entries.
        /// </param>
        private SimpleContainer(IEnumerable<ContainerEntry> entries)
        {
            this.mEntries = new List<ContainerEntry>(entries);
        }

        /// <summary>
        /// Creates a child container.
        /// </summary>
        /// <returns>A new container.</returns>
        public SimpleContainer CreateChildContainer()
        {
            return new SimpleContainer(this.mEntries);
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

            return this.mEntries.Any(x => x.Service == service && x.Key == key);
        }

        /// <summary>
        /// Determines if a handler for the service/key has previously been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>True if a handler is registered; false otherwise.</returns>
        public bool IsRegistered<TService>(string key = null)
        {
            return this.IsRegistered(typeof(TService), key);
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
            this.GetOrCreateEntry(service, key).Add(c => singleton ?? (singleton = c.BuildInstance(implementation)));
        }

        /// <summary>
        /// Registers the class so that it is created once, on first request, and the same instance is returned to all requestors thereafter.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="key">The key.</param>
        public void RegisterSingleton<TImplementation>(string key = null)
        {
            this.RegisterSingleton(typeof(TImplementation), key, typeof(TImplementation));
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
            this.RegisterSingleton(typeof(TService), key, typeof(TImplementation));
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
            this.GetOrCreateEntry(typeof(TService), key).Add(c => singleton ?? (singleton = handler(c)));
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

            this.GetOrCreateEntry(service, key).Add(c => instance);
        }

        /// <summary>
        /// Registers an instance with the container.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="instance">The instance.</param>
        public void RegisterInstance<TService>(string key, TService instance)
        {
            this.RegisterInstance(typeof(TService), key, instance);
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

            this.GetOrCreateEntry(service, key).Add(c => c.BuildInstance(implementation));
        }

        /// <summary>
        /// Registers the class so that a new instance is created on each request.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        public void RegisterPerRequest<TService>(string key = null)
        {
            this.RegisterPerRequest<TService, TService>(key);
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
            this.RegisterPerRequest(typeof(TService), key, typeof(TImplementation));
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

            this.GetOrCreateEntry(service, key).Add(handler);
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

            this.GetOrCreateEntry(typeof(TService), key).Add(c => handler(c));
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

            var entry = this.mEntries.FirstOrDefault(x => x.Service == service && x.Key == key);
            if (entry == null)
            {
                return false;
            }

            return this.mEntries.Remove(entry);
        }

        /// <summary>
        /// Unregisters any handlers for the service/key that have previously been registered.
        /// </summary>
        /// <typeparam name="TService">The of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>true if handler is successfully removed; otherwise, false.</returns>
        public bool UnregisterHandler<TService>(string key = null)
        {
            return this.UnregisterHandler(typeof(TService), key);
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

            var entry = this.mEntries.FirstOrDefault(x => x.Service == service && x.Key == key) ?? this.mEntries.FirstOrDefault(x => x.Service == service);
            if (entry != null)
            {
                if (entry.Count != 1)
                {
                    throw new InvalidOperationException(
                        string.Format("Found multiple registrations for type '{0}' and key {1}.", service, key));
                }

                return entry[0](this);
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
                    throw new InvalidOperationException(
                        string.Format("Requesting type '{0}' with key {1} is not supported.", service, key));
                }

                var listType = service.GenericTypeArguments[0];
                var instances = this.GetAllInstances(listType);
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
            return (TService)this.GetInstance(typeof(TService), key);
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

            var instances = this.mEntries
                .Where(x => x.Service == service)
                .SelectMany(e => e.Select(x => x(this)))
                .ToArray();

            return instances;
        }

        /// <summary>
        /// Gets all instances of a particular type.
        /// </summary>
        /// <typeparam name="TService">The type to resolve.</typeparam>
        /// <returns>The resolved instances.</returns>
        public TService[] GetAllInstances<TService>()
        {
            var service = typeof(TService);

            var instances = this.mEntries
                .Where(x => x.Service == service)
                .SelectMany(e => e.Select(x => (TService)x(this)))
                .ToArray();

            return instances;
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
                throw new InvalidOperationException(string.Format("Type '{0}' has no public constructor.", type));
            }

            var args = constructor.GetParameters()
                .Select(info => this.GetInstance(info.ParameterType))
                .ToArray();

            return this.ActivateInstance(type, args);
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
            var entry = this.mEntries.FirstOrDefault(x => x.Service == service && x.Key == key);
            if (entry == null)
            {
                entry = new ContainerEntry { Service = service, Key = key };
                this.mEntries.Add(entry);
            }

            return entry;
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