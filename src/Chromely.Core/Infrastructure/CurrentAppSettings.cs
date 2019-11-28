using Chromely.Core.Defaults;

namespace Chromely.Core.Infrastructure
{
    public class CurrentAppSettings : chromely
    {
        private IChromelyAppSettings appSettings;

        public override IChromelyAppSettings Properties
        {
            get
            {
                if (appSettings == null)
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
}
