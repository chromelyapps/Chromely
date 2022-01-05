// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Logging;

public abstract class Logger
{
    private static Logger? instance;
    public static Logger Instance
    {
        get
        {
            if (instance is null)
            {
                //Ambient Context can't return null, so we assign Local Default
                instance = new DefaultLogger();
            }

            return instance;
        }
        set
        {
            instance = (value is null) ? new DefaultLogger() : value;
        }
    }
#nullable disable
    public virtual ILogger Log { get; set; }
#nullable restore
}
