// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Chromely.Core.Infrastructure
{
    public class ChromelyDynamic : DynamicObject
    {
        public ChromelyDynamic()
        {
            Dictionary = new Dictionary<string, object>();
        }
        public ChromelyDynamic(IDictionary<string, object> dictionary)
        {
            Dictionary = dictionary;
        }

        public IDictionary<string, object> Dictionary { get; }

        public bool Empty
        {
            get
            {
                return Dictionary == null || !Dictionary.Any();
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (Dictionary.ContainsKey(binder.Name))
            {
                result = Dictionary[binder.Name];
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Dictionary[binder.Name] = value;
            return true;
        }
    }
}

