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
        _routeArguments = null;
        _queryParameterArgs = null;
        _propertyNameArgumentMap = null;
    }

    public string Name { get; set; }
    public dynamic Delegate { get; set; }
    public IList<RouteArgument> RouteArguments { get; set; }
    public bool IsAsync { get; set; }
    public bool HasReturnValue { get; set; }

    #region Invokes
    public IChromelyResponse Invoke(IChromelyRequest request)
    {
        if (Delegate == null)
        {
            return new ChromelyResponse()
            {
                HasRouteResponse = HasReturnValue
            };
        }

        SetRouteArguments(request);
        _routeArguments = _routeArguments ?? new object[] { };
        object content = Invoke(_routeArguments.Length, _routeArguments);

        return CreateResponse(content);
    }

    public async Task<IChromelyResponse> InvokeAsync(IChromelyRequest request)
    {
        var response = await InvokeLocal();
        return response;

        Task<IChromelyResponse> InvokeLocal()
        {
            var responseLocal = Invoke(request);
            return Task.FromResult<IChromelyResponse>(responseLocal);
        }
    }

    #endregion

    #region Helper Methods

    private IChromelyResponse CreateResponse(object content)
    {
        IChromelyResponse chromelyResponse = content as IChromelyResponse;
        if (chromelyResponse == null && HasReturnValue)
        {
            var task = content as Task;
            if (task != null)
            {
                switch (task.Status)
                {
                    case TaskStatus.RanToCompletion:
                        {
                            var resultProperty = task.GetType().GetProperty("Result");
                            chromelyResponse = resultProperty.GetValue(task) as IChromelyResponse;
                            if (chromelyResponse == null)
                            {
                                chromelyResponse = new ChromelyResponse();
                                chromelyResponse.Data = resultProperty.GetValue(task);
                            }

                            chromelyResponse.Status     = (int)HttpStatusCode.OK;
                            chromelyResponse.StatusText = ResourceConstants.StatusOKText;
                            break;
                        }

                    default:
                        {
                            chromelyResponse = new ChromelyResponse();
                            chromelyResponse.Status = (int)HttpStatusCode.BadRequest;
                            var builder = new StringBuilder();
                            var statusText = task.Status.ToString();
                            builder.AppendLine($"{statusText}");
                            if (task.Exception != null)
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

        if (chromelyResponse == null)
        {
            chromelyResponse = new ChromelyResponse();
            chromelyResponse.Data = HasReturnValue ? content : null;
        }

        return chromelyResponse;
    }

    private void SetRouteArguments(IChromelyRequest request)
    {
        ValidateRequest(request);

        int argumentsCount = 0;
        _propertyNameArgumentMap = null;
        _routeArguments = SetArgumentDefaultValues(out argumentsCount, out _propertyNameArgumentMap);

        if (argumentsCount == 0)
        {
            return;
        }

        _queryParameterArgs = GetQueryParameterArgs(request.Parameters);

        if (request.PostData != null)
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

        var jsonDocumentOptions = _dataTransfers.SerializerOptions.DocumentOptions();
        using (JsonDocument jsonDocument = JsonDocument.Parse(json, jsonDocumentOptions))
        {
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

    private Dictionary<string, object> GetQueryParameterArgs(IDictionary<string, object> queryParameters)
    {
        var argDic = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        if (queryParameters == null || !queryParameters.Any())
        {
            return argDic;
        }

        var dictQueryParamIgnoreCase = new Dictionary<string, object>(queryParameters, StringComparer.InvariantCultureIgnoreCase);
        foreach (var argument in RouteArguments)
        {
            if (dictQueryParamIgnoreCase.ContainsKey(argument.PropertyName))
            {
                var argItemValue = dictQueryParamIgnoreCase[argument.PropertyName];
                if (argItemValue != null)
                {
                    if (argItemValue is JsonElement)
                    {
                        argItemValue = ((JsonElement)argItemValue).JsonToObject();
                    }

                    if (argument.Type.IsArrayType())
                    {
                        var argItemValueList = argItemValue as IList<object>;
                        if (argItemValueList == null)
                        {
                            // If argument value is comma separated
                            // like "one,two,three"
                            argItemValueList = argItemValue.ToString().Split(',');
                        }

                        if (argItemValueList != null && argItemValueList.Any())
                        {
                            Type subType = argument.Type.ArrayElementType();
                            var tempList = subType.CreateGenericList();
                            foreach (var argItem in argItemValueList)
                            {
                                tempList.Add(argItem.ChangeObjectType(subType));
                            }
                            argDic[argument.PropertyName] = tempList;
                        }
                    }
                    else if (argument.Type.IsDictionaryType())
                    {
                        var argItemValueDic = argItemValue as IDictionary<string, object>;
                        if (argItemValueDic != null && argItemValueDic.Any())
                        {
                            var type1 = argument.Type.DictionaryElementKeyType();
                            var type2 = argument.Type.DictionaryElementValueType();
                            Type dictType = typeof(Dictionary<,>).MakeGenericType(type1, type2);
                            var tempDic = Activator.CreateInstance(dictType);

                            foreach (var argItem in argItemValueDic)
                            {
                                var addToDic = tempDic.GetType().GetMethod("Add", new[] {type1, type2 });
                                addToDic.Invoke(tempDic, new object[] { argItem.Key, argItem.Value.ChangeObjectType(type2) });
                            }

                            argDic[argument.PropertyName] = tempDic;
                        }
                    }
                    else
                    {
                        argDic[argument.PropertyName] = argItemValue.ChangeObjectType(argument.Type);
                    }
                }
            }
        }

        return argDic;
    }

    private void ValidateRequest(IChromelyRequest request)
    {
        if (request == null)
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
            return new object[] { };
        }

        argumentsCount = RouteArguments.Count;
        var args = new object[argumentsCount];
        foreach (var argument in RouteArguments)
        {
            args[argument.Index] = args[argument.Index].ChangeObjectType(argument.Type);
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
                        var del = Delegate as Delegate;
                        if (del != null)
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
                        var del = Delegate as Delegate;
                        if (del != null)
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