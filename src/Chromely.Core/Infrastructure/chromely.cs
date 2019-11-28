using System;
using System.Collections.Generic;
using System.Text;

namespace Chromely.Core.Infrastructure
{
    public abstract class chromely
    {
        private static chromely instance;
        public static chromely App
        {
            get
            {
                if (instance == null)
                {
                    //Ambient Context can't return null, so we assign Local Default
                    instance = new CurrentAppSettings();
                }

                return instance;
            }
            set
            {
                instance = (value == null) ? new CurrentAppSettings() : value;
            }
        }

        public virtual IChromelyAppSettings Properties { get; set; }
        
    }
}

