// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

public class RouteArgument
{
    public RouteArgument(string propertyName, Type type, int index)
    {
        PropertyName = propertyName;
        Type = type;
        Index = index;
    }

    public string PropertyName { get; set; }
    public Type Type { get; set; }
    public int Index { get; set; }
}