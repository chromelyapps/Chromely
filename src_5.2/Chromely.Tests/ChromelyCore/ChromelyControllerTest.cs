// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Tests.ChromelyCore;
public class ChromelyControllerTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IChromelyRouteProvider _routeProvider;
    private readonly List<ChromelyController> _controllers;

    public ChromelyControllerTests(IServiceProvider serviceProvider, IChromelyRouteProvider routeProvider)
    {
        _serviceProvider = serviceProvider;
        _routeProvider = routeProvider;
        _controllers = serviceProvider.GetServices<ChromelyController>().ToList();
        routeProvider.RegisterAllRoutes(_controllers);
    }

    [Fact]
    public void TodoController_Routes_Registered_Test()
    {
        Assert.NotNull(_controllers);
        Assert.True(_controllers.Any());
        Assert.True(Attribute.IsDefined(_controllers[0].GetType(), typeof(ChromelyControllerAttribute)));
        bool todoControllerFound = _controllers.Any(x => x is TodoController);
        Assert.True(todoControllerFound);
    }

    /// <summary>
    /// The route count test.
    /// </summary>
    [Fact]
    public void RouteCountTest()
    {
        var todoRouteCount = TodoController.GetRoutePaths.Count;
        var todoRouteCountRegistered = 0;

        foreach (var routePathItem in TodoController.GetRoutePaths)
        {
            if (_routeProvider.RouteExists("http://chromely.com" + routePathItem.Value))
            {
                todoRouteCountRegistered++;
            }
        }

        Assert.Equal(todoRouteCount, todoRouteCountRegistered);
    }

    [Fact]
    public void CreateUpdateGetDeleteTest()
    {
        var todoItem = TodoItem.FakeTodoItem;

        // Create
        dynamic postData = new ExpandoObject();
        postData.todoItem = todoItem;

        var route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.CreateItem]);
        Assert.NotNull(route);

        var createRequest = new ChromelyRequest();
        createRequest.PostData = JsonSerializer.Serialize(postData);
        var createResponse = route?.Invoke(createRequest) as IChromelyResponse;
        var createData = createResponse is not null && createResponse.Data is int ? (int)createResponse.Data : -1; 

        Assert.NotNull(createResponse);
        Assert.True(createData > 0);

        // Update
        var newItemName = TodoItem.FakeTodoItemName;
        todoItem.Name = newItemName;
        postData = new ExpandoObject();
        postData.id = todoItem.Id;
        postData.todoItem = todoItem;

        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.UpdateItem]);
        Assert.NotNull(route);

        var updateRequest = new ChromelyRequest();
        updateRequest.PostData = JsonSerializer.Serialize(postData);
        var updateResponse = route?.Invoke(updateRequest) as IChromelyResponse;
        var updateData = updateResponse is not null && updateResponse.Data is int ? (int)updateResponse.Data : -1;

        Assert.NotNull(updateResponse);
        Assert.True(updateData > 0);

        // Get
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItem]);
        Assert.NotNull(route);

        var getRequest = new ChromelyRequest();
        getRequest.RouteUrl = $"{TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItem]}?id={todoItem.Id}";
        getRequest.Parameters = getRequest.RouteUrl.GetParameters();
        var getResponse = route?.Invoke(getRequest) as IChromelyResponse;
        TodoItem? getData = getResponse?.Data as TodoItem;

        Assert.NotNull(getResponse);
        Assert.NotNull(getData);

        if (getData is not null)
        {
            Assert.Equal(todoItem.Id, getData.Id);
            Assert.Equal(todoItem.Name, getData.Name);
            Assert.Equal(todoItem.IsComplete, getData.IsComplete);
        }

        // Delete
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.DeleteItem]);
        Assert.NotNull(route);

        var deleteRequest = new ChromelyRequest();
        deleteRequest.RouteUrl = $"{TodoController.GetRoutePaths[TodoControllerRouteKeys.DeleteItem]}?id={todoItem.Id}";
        deleteRequest.Parameters = getRequest.RouteUrl.GetParameters();
        var deleteResponse = route?.Invoke(deleteRequest) as IChromelyResponse;
        var deleteData = deleteResponse is not null && deleteResponse.Data is int ? (int)deleteResponse.Data : -1;

        Assert.NotNull(deleteResponse);
        Assert.True(deleteData > 0);

        // Get: Ensure delete
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItem]);
        Assert.NotNull(route);

        getRequest = new ChromelyRequest();
        getRequest.RouteUrl = $"{TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItem]}?id={todoItem.Id}";
        getRequest.Parameters = getRequest.RouteUrl.GetParameters();
        getResponse = route?.Invoke(getRequest) as IChromelyResponse;
        getData = getResponse?.Data as TodoItem;

        Assert.NotNull(getResponse);
        Assert.Null(getData);
    }

    [Fact]
    public async Task CreateUpdateGetDeleteAsyncTestAsync()
    {
        var todoItem = TodoItem.FakeTodoItem;

        // Create
        dynamic postData = new ExpandoObject();
        postData.todoItem = todoItem;

        var route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.CreateItemAsync]);
        Assert.NotNull(route);

        var createRequest = new ChromelyRequest();
        createRequest.PostData = JsonSerializer.Serialize(postData);
        var createResponse = await route.InvokeAsync(createRequest) as IChromelyResponse;
        var createData = (int)createResponse.Data;

        Assert.NotNull(createResponse);
        Assert.True(createData > 0);

        // Update
        var newItemName = TodoItem.FakeTodoItemName;
        todoItem.Name = newItemName;
        postData = new ExpandoObject();
        postData.id = todoItem.Id;
        postData.todoItem = todoItem;

        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.UpdateItemAsync]);
        Assert.NotNull(route);

        var updateRequest = new ChromelyRequest();
        updateRequest.PostData = JsonSerializer.Serialize(postData);
        var updateResponse = await route.InvokeAsync(updateRequest) as IChromelyResponse;
        var updateData = (int)updateResponse.Data;

        Assert.NotNull(updateResponse);
        Assert.True(updateData > 0);

        // Get
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItemAsync]);
        Assert.NotNull(route);

        var getRequest = new ChromelyRequest();
        getRequest.RouteUrl = $"{TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItemAsync]}?id={todoItem.Id}";
        getRequest.Parameters = getRequest.RouteUrl.GetParameters();
        var getResponse = await route.InvokeAsync(getRequest) as IChromelyResponse;
        var getData = getResponse.Data as TodoItem;

        Assert.NotNull(getResponse);
        Assert.NotNull(getData);
        Assert.Equal(todoItem.Id, getData.Id);
        Assert.Equal(todoItem.Name, getData.Name);
        Assert.Equal(todoItem.IsComplete, getData.IsComplete);

        // Delete
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.DeleteItemAsync]);
        Assert.NotNull(route);

        var deleteRequest = new ChromelyRequest();
        deleteRequest.RouteUrl = $"{TodoController.GetRoutePaths[TodoControllerRouteKeys.DeleteItemAsync]}?id={todoItem.Id}";
        deleteRequest.Parameters = getRequest.RouteUrl.GetParameters();
        var deleteResponse = await route.InvokeAsync(deleteRequest) as IChromelyResponse;
        var deleteData = (int)deleteResponse.Data;

        Assert.NotNull(deleteResponse);
        Assert.True(deleteData > 0);

        // Get: Ensure delete
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItemAsync]);
        Assert.NotNull(route);

        getRequest = new ChromelyRequest();
        getRequest.RouteUrl = $"{TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItemAsync]}?id={todoItem.Id}";
        getRequest.Parameters = getRequest.RouteUrl.GetParameters();
        getResponse = await route.InvokeAsync(getRequest) as IChromelyResponse;
        getData = getResponse.Data as TodoItem;

        Assert.NotNull(getResponse);
        Assert.Null(getData);
    }

    [Fact]
    public void GetAllTodoItemsTest()
    {
        var route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetAllItems]);
        Assert.NotNull(route);

        var request1 = new ChromelyRequest();
        var response1 = route.Invoke(request1) as IChromelyResponse;
        var data1 = response1.Data as List<TodoItem>;

        Assert.NotNull(response1);
        Assert.NotNull(data1);

        var startCount = data1.Count;

        // Add an item 
        var todoItem = TodoItem.FakeTodoItem;
        dynamic postData = new ExpandoObject();
        postData.todoItem = todoItem;

        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.CreateItem]);
        Assert.NotNull(route);

        var request2 = new ChromelyRequest();
        request2.PostData = JsonSerializer.Serialize(postData);
        var response2 = route.Invoke(request2) as IChromelyResponse;
        var data2 = (int)response2.Data;

        Assert.NotNull(response2);
        Assert.True(data2 > 0);

        // Get a new list
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetAllItems]);
        Assert.NotNull(route);

        request1 = new ChromelyRequest();
        response1 = route.Invoke(request1) as IChromelyResponse;
        data1 = response1.Data as List<TodoItem>;

        Assert.NotNull(response1);
        Assert.NotNull(data1);

        var endCount = data1.Count;

        Assert.Equal(startCount + 1, endCount);
    }

    [Fact]
    public async Task GetAllTodoItemsAsyncTestAsync()
    {
        var route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetAllItemsAsync]);
        Assert.NotNull(route);

        var request1 = new ChromelyRequest();
        var response1 = await route.InvokeAsync(request1) as IChromelyResponse;
        var data1 = response1.Data as List<TodoItem>;

        Assert.NotNull(response1);
        Assert.NotNull(data1);

        var startCount = data1.Count;

        // Add an item 
        var todoItem = TodoItem.FakeTodoItem;
        dynamic postData = new ExpandoObject();
        postData.todoItem = todoItem;

        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.CreateItemAsync]);
        Assert.NotNull(route);

        var request2 = new ChromelyRequest();
        request2.PostData = JsonSerializer.Serialize(postData);
        var response2 = await route.InvokeAsync(request2) as IChromelyResponse;
        var data2 = (int)response2.Data;

        Assert.NotNull(response2);
        Assert.True(data2 > 0);

        // Get a new list
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetAllItemsAsync]);
        Assert.NotNull(route);

        request1 = new ChromelyRequest();
        response1 = await route.InvokeAsync(request1) as IChromelyResponse;
        data1 = response1.Data as List<TodoItem>;

        Assert.NotNull(response1);
        Assert.NotNull(data1);

        var endCount = data1.Count;

        Assert.Equal(startCount + 1, endCount);
    }
}
