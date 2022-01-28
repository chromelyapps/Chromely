// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core;

/// <inheritdoc/>
public class ChromelyJsBindingHandler : IChromelyJsBindingHandler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChromelyJsBindingHandler"/> class.
    /// </summary>
    public ChromelyJsBindingHandler()
    {
        Key = Guid.NewGuid().ToString();
        ObjectName = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChromelyJsBindingHandler"/> class.
    /// </summary>
    /// <param name="objectName">The binding object name.</param>
    public ChromelyJsBindingHandler(string objectName)
    {
        Key = objectName;
        ObjectName = objectName;
        BoundObject = null;
        BindingOptions = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChromelyJsBindingHandler"/> class.
    /// </summary>
    /// <param name="objectName">The binding object name.</param>
    /// <param name="boundObject">The binding object.</param>
    /// <param name="bindingOptions">The binding object options.</param>
    public ChromelyJsBindingHandler(string objectName, object boundObject, object bindingOptions)
    {
        Key = objectName;
        ObjectName = objectName;
        BoundObject = boundObject;
        BindingOptions = bindingOptions;
    }

    /// <inheritdoc/>
    public string Key { get; }

    /// <inheritdoc/>
    public string ObjectName { get; set; }

    /// <inheritdoc/>
    public object? BoundObject { get; set; }

    /// <inheritdoc/>
    public object? BindingOptions { get; set; }
}