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

        public virtual IChromelyLogger Log { get; set; }
    }
}

