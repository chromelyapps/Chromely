// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Core.Network;

public abstract class ChromelyController
{
    private string _routePath;
    private string _name;
    private string _description;

    public string RoutePath
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_routePath))
            {
                SetAttributeInfo();
            }

            return _routePath;
        }
    }

    public string Name
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_name))
            {
                SetAttributeInfo();
            }

            return _name;
        }
    }

    public string Description
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_description))
            {
                SetAttributeInfo();
            }

            return _description;
        }
    }

    private void SetAttributeInfo()
    {
        try
        {
            var attribute = GetType().GetCustomAttribute<ChromelyControllerAttribute>(true);
            if (attribute != null)
            {
                _routePath = attribute.RoutePath;
                _name = attribute.Name;
                _description = attribute.Description;
            }
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }
    }
}