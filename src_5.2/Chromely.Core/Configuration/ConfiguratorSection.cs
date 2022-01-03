// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace Chromely.Core.Configuration
{
    public class ConfiguratorSection
    {
        public string? AppName { get; set; }
        public string? StartUrl { get; set; }
        public string? ChromelyVersion { get; set; }
        public bool DebuggingMode { get; set; }
        public string? DevToolsUrl { get; set; }
        public Dictionary<string, string>? CommandLineArgs { get; set; }
        public List<string>? CommandLineOptions { get; set; }
        public Dictionary<string, string>? CustomSettings { get; set; }
        public Dictionary<string, object>? ExtensionData { get; set; }
    }
}