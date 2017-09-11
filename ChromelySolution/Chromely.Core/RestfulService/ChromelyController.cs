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

    public abstract class ChromelyController
    {
        public ChromelyController()
        {
            RouteDictionary = new Dictionary<string, Route>();
        }

        public virtual string ControllerPath { get; private set; }
        public virtual Dictionary<string, Route> RouteDictionary { get; private set; }

        protected virtual void RegisterGetRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(path, new Route(Method.GET, path, action));
        }

        protected virtual void RegisterPostRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(path, new Route(Method.POST, path, action));
        }

        protected virtual void RegisterPutRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(path, new Route(Method.PUT, path, action));
        }

        protected virtual void RegisterDeleteRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(path, new Route(Method.DELETE, path, action));
        }

        protected virtual void RegisterHeadRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(path, new Route(Method.HEAD, path, action));
        }

        protected virtual void RegisterOptionsRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(path, new Route(Method.OPTIONS, path, action));
        }

        protected virtual void RegisterPatchRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(path, new Route(Method.PATCH, path, action));
        }

        protected virtual void RegisterMergeRequest(string path, Func<ChromelyRequest, ChromelyResponse> action)
        {
            AddRoute(path, new Route(Method.MERGE, path, action));
        }

        protected virtual void AddRoute(string path, Route route)
        {
            RouteDictionary[path] = route;
        }
    }
}
