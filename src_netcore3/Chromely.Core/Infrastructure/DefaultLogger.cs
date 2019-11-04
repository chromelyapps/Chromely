using System;
using System.Collections.Generic;
using System.Text;

namespace Chromely.Core.Infrastructure
{
    public class DefaultLogger : Logger
    {
        private IChromelyLogger chromelyLogger;

        public override IChromelyLogger Log
        {
            get 
            { 
                if (chromelyLogger == null)
                {
                    chromelyLogger = new SimpleLogger();
                }

                return chromelyLogger; 
            }
            set
            {
                chromelyLogger = value;
            }
        }
    }
}
