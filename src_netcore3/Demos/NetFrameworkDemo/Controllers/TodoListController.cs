using System;
using System.Collections.Generic;
using System.Linq;
using Chromely.Core.Network;

namespace NetFrameworkDemo.Controllers
{
    [ControllerProperty(Name = "TodoListController", Route = "todolistcontroller")]
    public class TodoListController : ChromelyController
    {
        private static readonly object _lockObj = new object();
        private List<TodoItem> _todoItemList;
        private const int StartId = 1000;

        public TodoListController()
        {
            _todoItemList = new List<TodoItem>();
        }

        #region HttpAttributes

        [HttpGet(Route = "/todolistcontroller/items")]
        public ChromelyResponse GetTodoItems(ChromelyRequest request)
        {
            var parameters = request.Parameters as IDictionary<string, string>;
            var name = string.Empty;
            var id = string.Empty;
            var todo = string.Empty;
            var completed = string.Empty;

            if (parameters != null && parameters.Any())
            {
                if (parameters.ContainsKey("name")) name = parameters["name"] ?? string.Empty;
                if (parameters.ContainsKey("id")) id = parameters["id"] ?? string.Empty;
                if (parameters.ContainsKey("todo")) todo = parameters["todo"] ?? string.Empty;
                if (parameters.ContainsKey("completed")) completed = parameters["completed"] ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return new ChromelyResponse() { RequestId = request.Id, Data = new List<Result>() };
            }

            int identifier = 0;
            int.TryParse(id, out identifier);

            int intCompleted = 0;
            int.TryParse(completed, out intCompleted);
            intCompleted = intCompleted == 1 ? 1 : 0;

            TodoItem todoItem = new TodoItem(identifier, todo, intCompleted);

            var todoItems = new List<TodoItem>();
            switch (name.ToLower())
            {
                case "add":
                    todoItems = GetOrUpdateList(RequestType.Add, todoItem);
                    break;
                case "delete":
                    todoItems = GetOrUpdateList(RequestType.Delete, todoItem);
                    break;
                case "all":
                    todoItems = GetOrUpdateList(RequestType.All, todoItem);
                    break;
                case "allactive":
                    todoItems = GetOrUpdateList(RequestType.AllActive, todoItem);
                    break;
                case "allcompleted":
                    todoItems = GetOrUpdateList(RequestType.AllCompleted, todoItem);
                    break;
                case "clearcompleted":
                    todoItems = GetOrUpdateList(RequestType.ClearCompleted, todoItem);
                    break;
                case "toggleall":
                    todoItems = GetOrUpdateList(RequestType.ToggleAll, todoItem);
                    break;
            }

            return new ChromelyResponse() { RequestId = request.Id, Data = todoItems }; ;
        }


        #endregion

        #region CustomAttributes

        [Command(Route = "/todolistcontroller/toggleactive")]
        public void ToggleActive(IDictionary<string, string> queryParameters)
        {
            var id = string.Empty;
            var completed = string.Empty;

            if (queryParameters != null && queryParameters.Any())
            {
                if (queryParameters.ContainsKey("id")) id = queryParameters["id"] ?? string.Empty;
                if (queryParameters.ContainsKey("completed")) completed = queryParameters["completed"] ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                return;
            }

            int identifier = 0;
            int.TryParse(id, out identifier);

            int intCompleted = 0;
            int.TryParse(completed, out intCompleted);
            intCompleted = intCompleted == 1 ? 1 : 0;

            TodoItem todoItem = new TodoItem(identifier, string.Empty, intCompleted);

            GetOrUpdateList(RequestType.ToggleItemComplete, todoItem);
        }

        #endregion

        private List<TodoItem> GetOrUpdateList(RequestType requestType, TodoItem todoItem)
        {
            lock (_lockObj)
            {
                _todoItemList = _todoItemList ?? new List<TodoItem>();
                switch (requestType)
                {
                    case RequestType.Add:
                        int nextId = !_todoItemList.Any() ? StartId : _todoItemList.Select(x => x.Id).Max() + 1;
                        todoItem.Id = nextId;
                        if (todoItem != null && todoItem.Valid)
                        {
                            _todoItemList.Add(todoItem);
                            return _todoItemList.OrderByDescending(x => x.Id).ToList();
                        }
                        break;

                    case RequestType.Delete:
                        if (todoItem != null && todoItem.Id > 0)
                        {
                            var itemToRemove = _todoItemList.FirstOrDefault(x => x.Id == todoItem.Id);
                            if (itemToRemove != null)
                            {
                                _todoItemList.Remove(itemToRemove);
                                return _todoItemList.OrderByDescending(x => x.Id).ToList();
                            }
                        }
                        break;

                    case RequestType.All:
                        return _todoItemList.OrderByDescending(x => x.Id).ToList();

                    case RequestType.AllActive:
                        return _todoItemList.Where(aa => aa.Completed == 0).OrderByDescending(x => x.Id).ToList();

                    case RequestType.AllCompleted:
                        return _todoItemList.Where(aa => aa.Completed == 1).OrderByDescending(x => x.Id).ToList();

                    case RequestType.ClearCompleted:
                        _todoItemList.RemoveAll(x => x.Completed == 1);
                        return _todoItemList.OrderByDescending(x => x.Id).ToList();

                    case RequestType.ToggleAll:
                        _todoItemList.ForEach(x => x.Completed = todoItem.Completed);
                        return _todoItemList.OrderByDescending(x => x.Id).ToList();

                    case RequestType.ToggleItemComplete:
                        var itemToToggle = _todoItemList.FirstOrDefault(x => x.Id == todoItem.Id);
                        if (itemToToggle != null)
                        {
                            itemToToggle.Completed = todoItem.Completed;
                        }
                        return null;
                }
            }

            return _todoItemList ?? new List<TodoItem>();
        }
    }

    public enum RequestType
    {
        Add,
        Delete,
        All,
        AllActive,
        AllCompleted,
        ClearCompleted,
        ToggleAll,
        ToggleItemComplete
    }

    public class TodoItem
    {
        public TodoItem(int id, string todo, int completed)
        {
            Id = id;
            Todo = todo;
            Completed = completed;
            CreatedDate = DateTime.UtcNow;
        }

        public int Id { get; set; }

        public string Todo { get; set; }

        public int Completed { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool Valid
        { 
            get
            {
                return Id > 0 && !string.IsNullOrWhiteSpace(Todo);
            }
        }
    }
}



