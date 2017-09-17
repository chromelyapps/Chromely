namespace Chromely.CefSharpBrowser.RequestHandlers
{
    using Chromely.Core.RestfulService;
    using Chromely.RestfulService;
    using LitJson;
    using System.Diagnostics;

    public class CefSharpBoundObject
    {
        public class AsynCefSharpBoundObject
        {
            [DebuggerHidden]
            public string GetJson(string routePath, string parameters)
            {
                ChromelyResponse response = CefSharpRequestTaskRunner.Run(routePath, parameters, null);
                return JsonMapper.ToJson(response);
            }

            [DebuggerHidden]
            public string PostJson(string routePath, string parameters, object postData)
            {
                return postData.ToString();
            }

            [DebuggerHidden]
            public object Get(string routePath, string parameters)
            {
                return routePath + " " + parameters;
            }

            [DebuggerHidden]
            public object Post(string routePath, object parameters, object postData)
            {
                return postData.ToString();
            }
        }
    }
}
