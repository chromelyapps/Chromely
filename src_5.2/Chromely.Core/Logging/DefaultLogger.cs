// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Logging;

/// <summary>
/// Wrapper class to assign <see cref="SimpleLogger"/> or a custom implementation of <see cref="ILogger"/>.
/// </summary>
public class DefaultLogger : Logger
{
    private ILogger? _ChromelyLogger;

    /// <summary>
    /// Gets or sets logger.
    /// </summary>
    public override ILogger Log
    {
        get
        {
            if (_ChromelyLogger is null)
            {
                _ChromelyLogger = new SimpleLogger();
            }

            return _ChromelyLogger;
        }
        set
        {
            _ChromelyLogger = value;
        }
    }
}