// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Tests.ChromelyControllers;

[ChromelyController(Name = "TodoController")]
public class TodoController : ChromelyController
{
    private readonly IDbContextFactory<TodoContext> _contextFactory;

    public TodoController(IDbContextFactory<TodoContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    #region Sync Actions
    [ChromelyRoute(Path = "/todo/getall")]
    public IEnumerable<TodoItem> GetTodoItems()
    {
        return Task.Run(async () =>
        {
            return await GetTodoItemsAsync();
        }).Result;
    }

    [ChromelyRoute(Path = "/todo/get")]
    public TodoItem GetTodoItem(long id)
    {
        return Task.Run(async () =>
        {
            return await GetTodoItemAsync(id);
        }).Result;
    }

    [ChromelyRoute(Path = "/todo/create")]
    public int CreateTodoItem(TodoItem todoItem)
    {
        return Task.Run(async () =>
        {
            return await CreateTodoItemAsync(todoItem);
        }).Result;
    }

    [ChromelyRoute(Path = "/todo/update")]
    public int UpdateTodoItem(long id, TodoItem todoItem)
    {
        return Task.Run(async () =>
        {
            return await UpdateTodoItemAsync(id, todoItem);
        }).Result;
    }

    [ChromelyRoute(Path = "/todo/delete")]
    public int DeleteTodoItem(long id)
    {
        return Task.Run(async () =>
        {
            return await DeleteTodoItemAsync(id);
        }).Result;
    }

    #endregion Sync Actions

    #region Async Actions

    [ChromelyRoute(Path = "/todo/async/getall")]
    public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.TodoItems.ToListAsync();
    }

    [ChromelyRoute(Path = "/todo/async/get")]
    public async Task<TodoItem> GetTodoItemAsync(long id)
    {
        using var context = _contextFactory.CreateDbContext();
#pragma warning disable CS8603 // Possible null reference return.
        return await context.TodoItems.FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.
    }

    [ChromelyRoute(Path = "/todo/async/create")]
    public async Task<int> CreateTodoItemAsync(TodoItem todoItem)
    {
        using var context = _contextFactory.CreateDbContext();
        context.TodoItems.Add(todoItem);
        return await context.SaveChangesAsync();
    }

    [ChromelyRoute(Path = "/todo/async/update")]
    public async Task<int> UpdateTodoItemAsync(long id, TodoItem todoItem)
    {
        if (id != todoItem.Id)
        {
            return 0;
        }

        using var context = _contextFactory.CreateDbContext();
        context.Entry(todoItem).State = EntityState.Modified;

        try
        {
            return await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoItemExists(context, id))
            {
                return 0;
            }
            else
            {
                throw;
            }
        }
    }

    [ChromelyRoute(Path = "/todo/async/delete")]
    public async Task<int> DeleteTodoItemAsync(long id)
    {
        using var context = _contextFactory.CreateDbContext();
        var todoItem = await context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return 0;
        }

        context.TodoItems.Remove(todoItem);
        return await context.SaveChangesAsync();
    }

    #endregion Async Actions
    private bool TodoItemExists(TodoContext context, long id)
    {
       return context.TodoItems.Any(e => e.Id == id);
    }

    public static IDictionary<string, string> GetRoutePaths
    {
        get
        {
            return new Dictionary<string, string>()
            {
                { TodoControllerRouteKeys.GetAllItems,          "/todo/getall" },
                { TodoControllerRouteKeys.GetItem,              "/todo/get" },
                { TodoControllerRouteKeys.CreateItem,           "/todo/create" },
                { TodoControllerRouteKeys.UpdateItem,           "/todo/update" },
                { TodoControllerRouteKeys.DeleteItem,           "/todo/delete" },
                { TodoControllerRouteKeys.GetAllItemsAsync,     "/todo/async/getall" },
                { TodoControllerRouteKeys.GetItemAsync,         "/todo/async/get" },
                { TodoControllerRouteKeys.CreateItemAsync,      "/todo/async/create" },
                { TodoControllerRouteKeys.UpdateItemAsync,      "/todo/async/update" },
                { TodoControllerRouteKeys.DeleteItemAsync,      "/todo/async/delete" }
            };
        }
    }

}