// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

public class ControllerRoute
{
    private readonly IChromelyModelBinder _routeParameterBinder;
    private readonly IChromelyDataTransferOptions _dataTransfers;
    private object[] _routeArguments;
    private IDictionary<string, object> _queryParameterArgs;
    private IDictionary<string, RouteArgument> _propertyNameArgumentMap;

    public ControllerRoute(string name, dynamic del, IList<RouteArgument> argumentInfos, IChromelyModelBinder routeParameterBinder, IChromelyDataTransferOptions dataTransfers, bool isAsync, bool hasReturnValue)
    {
        Name = name;
        Delegate = del;
        RouteArguments = argumentInfos;
        _routeParameterBinder = routeParameterBinder;
        _dataTransfers = dataTransfers;
        IsAsync = isAsync;
        HasReturnValue = hasReturnValue;
        _routeArguments = new object[argumentInfos.Count];
        _queryParameterArgs = new Dictionary<string, object>();
        _propertyNameArgumentMap = new Dictionary<string, RouteArgument>();
    }

    public string Name { get; set; }
    public dynamic Delegate { get; set; }
    public IList<RouteArgument> RouteArguments { get; set; }
    public bool IsAsync { get; set; }
    public bool HasReturnValue { get; set; }

    #region Invokes

    public IChromelyResponse Invoke(IChromelyRequest request)
    {
        if (Delegate is null)
        {
            return new ChromelyResponse()
            {
                RequestId = request?.Id == null ? string.Empty : request.Id,
                HasRouteResponse = HasReturnValue
            };
        }

        SetRouteArguments(request);
        _routeArguments ??= Array.Empty<object>();
        object? content = Invoke(_routeArguments.Length, _routeArguments);

        return CreateResponse(content);
    }

    public async Task<IChromelyResponse> InvokeAsync(IChromelyRequest request)
    {
        if (Delegate is null)
        {
            return new ChromelyResponse()
            {
                RequestId = request?.Id == null ? string.Empty : request.Id,
                HasRouteResponse = HasReturnValue
            };
        }

        SetRouteArguments(request);
        _routeArguments ??= Array.Empty<object>();
        var content = await InvokeAsync(_routeArguments.Length, _routeArguments);
        return CreateResponse(content);
    }

    #endregion

    #region Helper Methods

    private IChromelyResponse CreateResponse(object? content)
    {
        IChromelyResponse? chromelyResponse = content as IChromelyResponse;
        if (chromelyResponse is null && HasReturnValue)
        {
            var task = content as Task;
            if (task is not null)
            {
                switch (task.Status)
                {
                    case TaskStatus.RanToCompletion:
                        {
                            var resultProperty = task.GetType().GetProperty("Result");
                            chromelyResponse = resultProperty?.GetValue(task) as IChromelyResponse;
                            if (chromelyResponse is null)
                            {
                                chromelyResponse = new ChromelyResponse
                                {
                                    Data = resultProperty?.GetValue(task)
                                };
                            }

                            chromelyResponse.Status     = (int)HttpStatusCode.OK;
                            chromelyResponse.StatusText = ResourceConstants.StatusOKText;
                            break;
                        }

                    default:
                        {
                            chromelyResponse = new ChromelyResponse
                            {
                                Status = (int)HttpStatusCode.BadRequest
                            };
                            var builder = new StringBuilder();
                            var statusText = task.Status.ToString();
                            builder.AppendLine($"{statusText}");
                            if (task.Exception is not null)
                            {
                                builder.AppendLine($"  - {task.Exception.Message}");
                                Logger.Instance.Log.LogError(task.Exception);
                            }
                            chromelyResponse.Data       = builder.ToString();
                            chromelyResponse.StatusText = statusText;
                            break;
                        }
                }
            }
        }

        if (chromelyResponse is null)
        {
            chromelyResponse = new ChromelyResponse
            {
                Data = HasReturnValue ? content : null
            };
        }

        // Set request id if available in request and not yet set in response
        var chromelyRequest = content as IChromelyRequest;
        if (chromelyRequest is not null && 
            !string.IsNullOrEmpty(chromelyRequest.Id) && 
            string.IsNullOrEmpty(chromelyResponse.RequestId))
        {
            chromelyResponse.RequestId = chromelyRequest.Id;
        }

        return chromelyResponse;
    }

    private void SetRouteArguments(IChromelyRequest request)
    {
        ControllerRoute.ValidateRequest(request);
        _propertyNameArgumentMap = new Dictionary<string, RouteArgument>();

        _routeArguments = SetArgumentDefaultValues(out int argumentsCount, out _propertyNameArgumentMap);

        if (argumentsCount == 0)
        {
            return;
        }

        _queryParameterArgs = GetQueryParameterArgs(request.Parameters);

        if (request.PostData is not null)
        {
            ParsePostData(request.PostData);
        }

        // Add query parameters if exist as query arguments but not parsed.
        foreach (var queryArg in _queryParameterArgs)
        {
            if (_propertyNameArgumentMap.ContainsKey(queryArg.Key))
            {
                var argument = _propertyNameArgumentMap[queryArg.Key];
                _routeArguments[argument.Index] = queryArg.Value;
            }
        }
    }

    private void ParsePostData(object postData)
    {
        var json = _dataTransfers.ConvertRequestToJson(postData);

        if (json is not null)
        {
            var jsonDocumentOptions = _dataTransfers.SerializerOptions.DocumentOptions();
            using JsonDocument jsonDocument = JsonDocument.Parse(json, jsonDocumentOptions);
            foreach (JsonProperty element in jsonDocument.RootElement.EnumerateObject())
            {
                if (_propertyNameArgumentMap.ContainsKey(element.Name))
                {
                    var argument = _propertyNameArgumentMap[element.Name];
                    _routeArguments[argument.Index] = _routeParameterBinder.Bind(argument.PropertyName, argument.Type, element.Value);
                    _queryParameterArgs.Remove(element.Name);
                }
            }
        }
    }

    private Dictionary<string, object> GetQueryParameterArgs(IDictionary<string, object>? queryParameters)
    {
        var argDic = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        if (queryParameters is null || !queryParameters.Any())
        {
            return argDic;
        }

        var dictQueryParamIgnoreCase = new Dictionary<string, object>(queryParameters, StringComparer.InvariantCultureIgnoreCase);
        foreach (var argument in RouteArguments)
        {
            if (dictQueryParamIgnoreCase.ContainsKey(argument.PropertyName))
            {
                var argItemValue = dictQueryParamIgnoreCase[argument.PropertyName];
                if (argItemValue is not null)
                {
                    if (argItemValue is JsonElement element)
                    {
                        argItemValue = element.JsonToObject();
                    }

                    if (argument.Type.IsArrayType())
                    {
                        if (argItemValue is not IList<object> argItemValueList)
                        {
                            // If argument value is comma separated
                            // like "one,two,three"
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                            argItemValueList = argItemValue.ToString().Split(',');
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                        }

                        if (argItemValueList is not null && argItemValueList.Any())
                        {
                            Type subType = argument.Type.ArrayElementType();
                            var tempList = subType.CreateGenericList();

                            if (tempList is not null)
                            {
                                foreach (var argItem in argItemValueList)
                                {
                                    tempList.Add(argItem.ChangeObjectType(subType));
                                }

                                argDic[argument.PropertyName] = tempList;
                            }
                        }
                    }
                    else if (argument.Type.IsDictionaryType())
                    {
                        var argItemValueDic = argItemValue as IDictionary<string, object>;
                        if (argItemValueDic is not null && argItemValueDic.Any())
                        {
                            var type1 = argument.Type.DictionaryElementKeyType();
                            var type2 = argument.Type.DictionaryElementValueType();
                            Type dictType = typeof(Dictionary<,>).MakeGenericType(type1, type2);
                            var tempDic = Activator.CreateInstance(dictType);

                            if (tempDic is not null)
                            {
                                foreach (var argItem in argItemValueDic)
                                {
                                    var addToDic = tempDic.GetType().GetMethod("Add", new[] { type1, type2 });
#pragma warning disable CS8601 // Possible null reference assignment.
                                    addToDic?.Invoke(tempDic, new object[] { argItem.Key, argItem.Value.ChangeObjectType(type2) });
#pragma warning restore CS8601 // Possible null reference assignment.
                                }

                                argDic[argument.PropertyName] = tempDic;
                            }
                        }
                    }
                    else
                    {
#pragma warning disable CS8601 // Possible null reference assignment.
                        argDic[argument.PropertyName] = argItemValue.ChangeObjectType(argument.Type);
#pragma warning restore CS8601 // Possible null reference assignment.
                    }
                }
            }
        }

        return argDic;
    }

    private static void ValidateRequest(IChromelyRequest request)
    {
        if (request is null)
        {
            throw new ArgumentException("Request cannot be null", nameof(request));
        }
    }

    private object[] SetArgumentDefaultValues(out int argumentsCount, out IDictionary<string, RouteArgument> dictIgnoreCase)
    {
        dictIgnoreCase = new Dictionary<string, RouteArgument>(StringComparer.InvariantCultureIgnoreCase);
        argumentsCount = 0;
        if (RouteArguments == null || !RouteArguments.Any())
        {
            return Array.Empty<object>();
        }

        argumentsCount = RouteArguments.Count;
        var args = new object[argumentsCount];
        foreach (var argument in RouteArguments)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            args[argument.Index] = args[argument.Index].ChangeObjectType(argument.Type);
#pragma warning restore CS8601 // Possible null reference assignment.
            dictIgnoreCase[argument.PropertyName] = argument;
        }

        return args;
    }

    #endregion

    #region Local Invokes 

    private object Invoke(int argumentCount, params object[] args)
    {
        if (HasReturnValue)
        {
            switch (argumentCount)
            {
                case 0: return Delegate();
                case 1: return Delegate((dynamic)args[0]);
                case 2: return Delegate((dynamic)args[0], (dynamic)args[1]);
                case 3: return Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2]);
                case 4: return Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3]);
                case 5: return Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4]);
                case 6: return Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5]);
                case 7: return Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6]);
                case 8: return Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7]);
                case 9: return Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8]);
                case 10: return Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9]);
                case 11: return Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10]);
                case 12: return Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11]);
                case 13: return Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11], (dynamic)args[12]);
                case 14: return Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11], (dynamic)args[12], (dynamic)args[13]);
                case 15: return Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11], (dynamic)args[12], (dynamic)args[13], (dynamic)args[14]);
                default:
                    {
                        if (Delegate is Delegate del)
                        {
                            return del.DynamicInvoke(args);
                        }

                        return null;
                    }
            }
        }
        else
        {
            switch (argumentCount)
            {
                case 0: Delegate(); break;
                case 1: Delegate((dynamic)args[0]); break;
                case 2: Delegate((dynamic)args[0], (dynamic)args[1]); break;
                case 3: Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2]); break;
                case 4: Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3]); break;
                case 5: Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4]); break;
                case 6: Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5]); break;
                case 7: Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6]); break;
                case 8: Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7]); break;
                case 9: Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8]); break;
                case 10: Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9]); break;
                case 11: Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10]); break;
                case 12: Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11]); break;
                case 13: Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11], (dynamic)args[12]); break;
                case 14: Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11], (dynamic)args[12], (dynamic)args[13]); break;
                case 15: Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11], (dynamic)args[12], (dynamic)args[13], (dynamic)args[14]); break;
                default:
                    {
                        if (Delegate is Delegate del)
                        {
                            return del.DynamicInvoke(args);
                        }

                        break;
                    }
            }
        }

        return null;
    }

    private async Task<object?> InvokeAsync(int argumentCount, params object[] args)
    {
        if (HasReturnValue)
        {
            switch (argumentCount)
            {
                case 0: return await Delegate();
                case 1: return await Delegate((dynamic)args[0]);
                case 2: return await Delegate((dynamic)args[0], (dynamic)args[1]);
                case 3: return await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2]);
                case 4: return await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3]);
                case 5: return await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4]);
                case 6: return await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5]);
                case 7: return await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6]);
                case 8: return await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7]);
                case 9: return await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8]);
                case 10: return await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9]);
                case 11: return await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10]);
                case 12: return await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11]);
                case 13: return await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11], (dynamic)args[12]);
                case 14: return await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11], (dynamic)args[12], (dynamic)args[13]);
                case 15: return await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11], (dynamic)args[12], (dynamic)args[13], (dynamic)args[14]);
                default:
                    {
                        if (Delegate is Delegate del)
                        {
                            return del.DynamicInvoke(args);
                        }

                        return null;
                    }
            }
        }
        else
        {
            switch (argumentCount)
            {
                case 0: await Delegate(); break;
                case 1: await Delegate((dynamic)args[0]); break;
                case 2: await Delegate((dynamic)args[0], (dynamic)args[1]); break;
                case 3: await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2]); break;
                case 4: await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3]); break;
                case 5: await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4]); break;
                case 6: await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5]); break;
                case 7: await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6]); break;
                case 8: await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7]); break;
                case 9: await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8]); break;
                case 10: await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9]); break;
                case 11: await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10]); break;
                case 12: await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11]); break;
                case 13: await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11], (dynamic)args[12]); break;
                case 14: await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11], (dynamic)args[12], (dynamic)args[13]); break;
                case 15: await Delegate((dynamic)args[0], (dynamic)args[1], (dynamic)args[2], (dynamic)args[3], (dynamic)args[4], (dynamic)args[5], (dynamic)args[6], (dynamic)args[7], (dynamic)args[8], (dynamic)args[9], (dynamic)args[10], (dynamic)args[11], (dynamic)args[12], (dynamic)args[13], (dynamic)args[14]); break;
                default:
                    {
                        if (Delegate is Delegate del)
                        {
                            return del.DynamicInvoke(args);
                        }

                        break;
                    }
            }
        }

        return null;
    }

    #endregion
}