// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

public class RouteDelegate
{
    public RouteDelegate(dynamic del, IList<RouteArgument> argumentInfos, int argumentCount, bool hasReturnValue)
    {
        Delegate = del;
        RouteArguments = argumentInfos;
        ArgumentCount = argumentCount;
        HasReturnValue = hasReturnValue;
    }

    public dynamic Delegate { get; set; }
    public IList<RouteArgument> RouteArguments { get; set; }
    public int ArgumentCount { get; set; }
    public bool HasReturnValue { get; set; }
}