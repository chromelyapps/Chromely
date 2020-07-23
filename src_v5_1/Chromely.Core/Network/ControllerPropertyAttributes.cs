// Copyright © 2017-2020 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;

namespace Chromely.Core.Network
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequestActionAttribute : Attribute
    {
        public string Name { get; set; }
        public string RouteKey { get; set; }
        public string Description { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class CommandActionAttribute : Attribute
    {
        public string Name { get; set; }
        public string RouteKey { get; set; }
        public string Description { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerPropertyAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}