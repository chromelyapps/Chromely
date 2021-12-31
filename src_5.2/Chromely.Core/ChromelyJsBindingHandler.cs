// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;

namespace Chromely.Core
{
    public class ChromelyJsBindingHandler : IChromelyJsBindingHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromelyJsBindingHandler"/> class.
        /// </summary>
        public ChromelyJsBindingHandler()
        {
            Key = Guid.NewGuid().ToString();
        }

        public ChromelyJsBindingHandler(string objectName)
        {
            Key = objectName;
            ObjectName = objectName;
            BoundObject = null;
            BindingOptions = null;
        }

        public ChromelyJsBindingHandler(string objectName, object boundObject, object bindingOptions, bool useDefaultObject)
        {
            Key = objectName;
            ObjectName = objectName;
            BoundObject = boundObject;
            BindingOptions = bindingOptions;
        }

        public string Key { get; }
        public string ObjectName{ get; set; }
        public object BoundObject { get; set; }
        public object BindingOptions { get; set; }
    }
}
