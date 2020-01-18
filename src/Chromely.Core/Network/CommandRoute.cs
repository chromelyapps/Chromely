using System;
using System.Collections.Generic;

namespace Chromely.Core.Network
{
    public class CommandRoute
    {
        public CommandRoute(string path, Action<IDictionary<string, string>> action)
        {
            Path = path;
            Key = GetKeyFromPath(path);
            Action = action;
        }

        public string Key { get; }
        public string Path { get; set; }
        public Action<IDictionary<string, string>> Action { get; set; }
        public void Invoke(IDictionary<string, string> queryParameters)
        {
             Action.Invoke(queryParameters);
        }

        public static string GetKeyFromPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            return $"command_{path}".Replace("/", "_").ToLower();
        }
    }
}
