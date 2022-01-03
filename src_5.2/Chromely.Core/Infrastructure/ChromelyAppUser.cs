// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Defaults;

namespace Chromely.Core.Infrastructure
{
    public abstract class ChromelyAppUser
    {
        private static ChromelyAppUser? instance;
        public static ChromelyAppUser App
        {
            get
            {
                if (instance is null)
                {
                    //Ambient Context can't return null, so we assign Local Default
                    instance = new CurrentAppSettings();
                }

                return instance;
            }
            set
            {
                instance = (value is null) ? new CurrentAppSettings() : value;
            }
        }

        public virtual IChromelyAppSettings Properties { get; set; } = new DefaultAppSettings();
    }
}

