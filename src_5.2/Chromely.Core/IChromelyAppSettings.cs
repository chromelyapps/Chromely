// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;

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
