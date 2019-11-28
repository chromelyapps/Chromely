using System.Collections.Generic;

namespace Chromely.Core
{
    public interface IChromelyAppSettings
    {
        string AppName { get; set; }
        string DataPath { get; }
        dynamic Settings { get; }
        void Read(IChromelyConfiguration config);
        void Save(IChromelyConfiguration config);
    }
}
