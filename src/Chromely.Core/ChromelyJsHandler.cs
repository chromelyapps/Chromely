// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChromelyJsHandler.cs" company="Chromely">
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

namespace Chromely.Core
{
    using System;

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
            this.Key = Guid.NewGuid().ToString();
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
            this.Key = objectNameToBind;
            this.ObjectNameToBind = objectNameToBind;
            this.BoundObject = null;
            this.RegisterAsAsync = registerAsync;
            this.BindingOptions = null;
            this.UseDefault = true;
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
            this.Key = objectNameToBind;
            this.ObjectNameToBind = objectNameToBind;
            this.BoundObject = boundObject;
            this.RegisterAsAsync = registerAsync;
            this.BindingOptions = bindingOptions;
            this.UseDefault = false;
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
