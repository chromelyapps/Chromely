namespace Chromely.Core.Infrastructure
{
    public abstract class Chromely
    {
        private static Chromely instance;
        public static Chromely App
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

