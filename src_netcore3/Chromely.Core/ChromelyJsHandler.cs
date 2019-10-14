// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyJsHandler.cs" company="Chromely Projects">
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
    /// The chromely Javascript bounded object handler registration.
    /// Currently only CefSharp object handling is supported.
    /// Cefsharp implementation details @ https://github.com/cefsharp/CefSharp/wiki/General-Usage
    /// </summary>
    public class ChromelyJsHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyJsHandler"/> class.
        /// </summary>
        public ChromelyJsHandler()
        {
            Key = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyJsHandler"/> class.
        /// </summary>
        /// <param name="objectNameToBind">
        /// The object name to bind.
        /// </param>
        /// <param name="registerAsync">
        /// If set to true, the handler will be registered the async version. 
        /// </param>
        public ChromelyJsHandler(string objectNameToBind, bool registerAsync)
        {
            Key = objectNameToBind;
            ObjectNameToBind = objectNameToBind;
            BoundObject = null;
            RegisterAsAsync = registerAsync;
            BindingOptions = null;
            UseDefault = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyJsHandler"/> class.
        /// </summary>
        /// <param name="objectNameToBind">
        /// The object name to bind.
        /// </param>
        /// <param name="boundObject">
        /// The bound object.
        /// </param>
        /// <param name="bindingOptions">
        /// The binding options.
        /// </param>
        /// <param name="registerAsync">
        /// If set to true, the handler will be registered the async version. 
        /// </param>
        public ChromelyJsHandler(string objectNameToBind, object boundObject, object bindingOptions, bool registerAsync)
        {
            Key = objectNameToBind;
            ObjectNameToBind = objectNameToBind;
            BoundObject = boundObject;
            RegisterAsAsync = registerAsync;
            BindingOptions = bindingOptions;
            UseDefault = false;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets or sets the object name to bind.
        /// </summary>
        public string ObjectNameToBind { get; set; }

        /// <summary>
        /// Gets or sets the bound object.
        /// </summary>
        public object BoundObject { get; set; }

        /// <summary>
        /// Gets or sets the binding options.
        /// </summary>
        public object BindingOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether register as async.
        /// </summary>
        public bool RegisterAsAsync { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use default.
        /// </summary>
        public bool UseDefault { get; set; }
    }
}
