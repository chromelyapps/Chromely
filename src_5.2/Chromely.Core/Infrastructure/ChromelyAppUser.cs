// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure;

/// <summary>
/// Ambient context class to manage instance of <see cref="IChromelyAppSettings"/>.
/// </summary>
public abstract class ChromelyAppUser
{
    private static ChromelyAppUser? instance;

    /// <summary>
    /// The <see cref="IChromelyAppSettings"/> instance.
    /// </summary>
    public static ChromelyAppUser App
    {
        get
        {
            if (instance is null)
            {
                // Ambient Context can't return null, so we assign Local Default
                instance = new CurrentAppSettings();
            }

            return instance;
        }
        set
        {
            instance = (value is null) ? new CurrentAppSettings() : value;
        }
    }

    /// <summary>
    /// Gets or sets the application settings.
    /// </summary>
    public virtual IChromelyAppSettings Properties { get; set; } = new DefaultAppSettings();
}
