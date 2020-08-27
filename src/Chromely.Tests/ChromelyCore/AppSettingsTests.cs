// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromely.Core.Configuration;
using Chromely.Core.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Chromely.Tests.ChromelyCore
{
    public class AppSettingsTests
    {
        [Fact]
        public void DefaultAppSettingsTest()
        {
            ChromelyAppUser.App.Properties.Settings.Item1 = "Market 01";
            ChromelyAppUser.App.Properties.Settings.Item2 = "Year 2020";

            var list = new List<string>();
            list.Add("item0001");
            list.Add("item0002");
            list.Add("item0003");
            ChromelyAppUser.App.Properties.Settings.TestItems = list;

            var config = new DefaultConfiguration();

            // Delete config file if exists
            DeleteConfigFile(config);

            // Save settings
            ChromelyAppUser.App.Properties.Save(config);

            // Read
            ChromelyAppUser.App.Properties.Settings.Item1 = null;
            ChromelyAppUser.App.Properties.Settings.Item2 = null;
            ChromelyAppUser.App.Properties.Settings.TestItems = null;
            ChromelyAppUser.App.Properties.Read(config);

            var item1 = (string)ChromelyAppUser.App.Properties.Settings.Item1;
            var item2 = (string)ChromelyAppUser.App.Properties.Settings.Item2;
            var testItems = (ArrayList)ChromelyAppUser.App.Properties.Settings.TestItems;
            Assert.NotNull(item1);
            Assert.NotNull(item2);
            Assert.NotNull(testItems);

            Assert.Equal("Market 01", item1);
            Assert.Equal("Year 2020", item2);
            Assert.Equal("item0001", testItems[0]);
            Assert.Equal("item0002", testItems[1]);
            Assert.Equal("item0003", testItems[2]);

            // Delete config file if exists
            // Delete config file if exists
            DeleteConfigFile(config);
        }

        private void DeleteConfigFile(IChromelyConfiguration config)
        {
            try
            {
                var appSettingsFile = AppSettingInfo.GetSettingsFilePath(config.Platform, config.AppName);
                if (!string.IsNullOrWhiteSpace(appSettingsFile))
                {
                    if (File.Exists(appSettingsFile))
                    {
                        File.Delete(appSettingsFile);
                    }
                }
            }
            catch {}
        }
    }
}
