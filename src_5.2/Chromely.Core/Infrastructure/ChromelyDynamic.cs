// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

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
                return Dictionary is null || !Dictionary.Any();
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
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

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            Dictionary[binder.Name] = value;
#pragma warning restore CS8601 // Possible null reference assignment.
            return true;
        }
    }
}

