// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Tests.Models;

public static class TodoControllerRouteKeys
{
    public const string GetAllItems = nameof(GetAllItems);
    public const string GetItem = nameof(GetItem);
    public const string CreateItem = nameof(CreateItem);
    public const string UpdateItem = nameof(UpdateItem);
    public const string DeleteItem = nameof(DeleteItem);

    public const string GetAllItemsAsync = nameof(GetAllItemsAsync);
    public const string GetItemAsync = nameof(GetItemAsync);
    public const string CreateItemAsync = nameof(CreateItemAsync);
    public const string UpdateItemAsync = nameof(UpdateItemAsync);
    public const string DeleteItemAsync = nameof(DeleteItemAsync);
}