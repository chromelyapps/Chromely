// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace Chromely.Core.Network
{
    public class CommandActionRoute
    {
        public CommandActionRoute(string path, Action<IDictionary<string, string>> action, string description = null)
        {
            Path = path;
            Action = action;
            Description = description;
        }

        public string Key { get; }
        public string Path { get; set; }
        public string Description { get; set; }
        public Action<IDictionary<string, string>> Action { get; set; }
        public void Invoke(IDictionary<string, string> queryParameters)
        {
            Action.Invoke(queryParameters);
        }
    }
}
