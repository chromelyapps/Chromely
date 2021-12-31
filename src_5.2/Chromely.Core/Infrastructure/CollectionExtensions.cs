// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure;

public static class CollectionExtensions
{
    public static bool NotEmpty(this IEnumerable<object> list)
    {
        return list != null && list.Any();
    }

    public static bool NotEmpty(this IList<object> list)
    {
        return list != null && list.Any();
    }

    public static bool IsNullOrEmpty(this IEnumerable<object> list)
    {
        return list == null || !list.Any();
    }

    public static bool IsNullOrEmpty(this IList<object> list)
    {
        return list == null || !list.Any();
    }
}