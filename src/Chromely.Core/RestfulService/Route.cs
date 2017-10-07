/**
 MIT License

 Copyright (c) 2017 Kola Oyewumi

 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all
 copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
 */

namespace Chromely.Core.RestfulService
{
    using System;
    using System.Collections.Generic;

    public class Route
    {
        public Route(Method method, string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            Method = method;
            Path = path;
            Action = action;
        }

        public Method Method { get; set; }
        public string Path { get; set; }
        public Func<ChromelyRequest, ChromelyResponse> Action { get; set; }

        public ChromelyResponse Invoke(string routePath, IDictionary<string, object> parameters, object postData)
        {
            ChromelyRequest request = new ChromelyRequest(routePath, parameters, postData);
            return Action.Invoke(request);
        }

        public ChromelyResponse Invoke(ChromelyRequest request)
        {
            return Action.Invoke(request);
        }
    }
}
