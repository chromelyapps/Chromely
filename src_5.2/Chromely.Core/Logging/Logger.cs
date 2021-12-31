// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Microsoft.Extensions.Logging;

namespace Chromely.Core.Logging
{
    public abstract class Logger
    {
        private static Logger instance;
        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    //Ambient Context can't return null, so we assign Local Default
                    instance = new DefaultLogger();
                }

                return instance;
            }
            set
            {
                instance = (value == null) ? new DefaultLogger() : value;
            }
        }

        public virtual ILogger Log { get; set; }
    }
}

