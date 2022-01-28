// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Infrastructure;

/// <summary>
/// Custom dynamic object class.
/// </summary>
public class ChromelyDynamic : DynamicObject
{
    /// <summary>
    /// Initializes a new instance of <see cref="ChromelyDynamic"/>.
    /// </summary>
    public ChromelyDynamic()
    {
        Dictionary = new Dictionary<string, object>();
    }

    /// <summary>
    /// Initializes a new instance of <see cref="ChromelyDynamic"/>.
    /// </summary>
    /// <param name="dictionary">Dictionary for creating and initializing a new dynamic object set.</param>
    public ChromelyDynamic(IDictionary<string, object> dictionary)
    {
        Dictionary = dictionary;
    }

    /// <summary>
    /// Gets the dynamic object as a dictionary.
    /// </summary>
    public IDictionary<string, object> Dictionary { get; }

    /// <summary>
    /// Gets a value indicating whether the dynamic object set is empty.
    /// </summary>
    public bool Empty
    {
        get
        {
            return Dictionary is null || !Dictionary.Any();
        }
    }

    /// <inheritdoc/>
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