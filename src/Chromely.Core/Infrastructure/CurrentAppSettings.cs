// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure;

/// <summary>
/// Wrapper class to assign <see cref="DefaultAppSettings"/> or a custom implementation of <see cref="IChromelyAppSettings"/>.
/// </summary>
public class CurrentAppSettings : ChromelyAppUser
{
    private IChromelyAppSettings? appSettings;

    /// <summary>
    /// Gets or sets the dynamic object as a set of "Properties".
    /// </summary>
    public override IChromelyAppSettings Properties
    {
        get
        {
            if (appSettings is null)
            {
                appSettings = new DefaultAppSettings();
            }

            return appSettings;
        }
        set
        {
            appSettings = value;
        }
    }
}
