namespace Chromely.Core.Logging
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
