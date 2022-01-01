// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Owin;

public static class OwinExtensions
{
    public static bool IsOwinApp(this ChromelyApp startup)
    {
        return typeof(IOwinAppStartup).IsAssignableFrom(startup.GetType());
    }

    public static bool IsClientErrorCode(this HttpStatusCode statusCode)
    {
        // 4xx client errors
        return (int)statusCode >= ResponseConstants.MinClientErrorStatusCode &&
               (int)statusCode <= ResponseConstants.MaxClientErrorStatusCode;
    }

    public static bool IsServerErrorCode(this HttpStatusCode statusCode)
    {
        // 5xx server errors
        return (int)statusCode >= ResponseConstants.MinServerErrorStatusCode;
    }

    public static string GetMimeType(this IDictionary<string, string[]> headers)
    {
       if (headers == null || !headers.Any())
       {
            return ResourceHandler.DefaultMimeType;
       }

        if (headers.ContainsKey(ResponseConstants.Header_ContentType))
        {
            var contentType = headers[ResponseConstants.Header_ContentType].First();
            return contentType.Split(';').First();
        }

        return ResourceHandler.DefaultMimeType;
    }

    public static void ParseRoutes(this IOwinPipeline owinPipeline, IServiceProvider provider)
    {
        try
        {
            owinPipeline.Routes = owinPipeline.Routes ?? new List<OwinRoute>();
            var routes = new List<OwinRoute>();

            var actionDescriptorCollectionProvider = provider.GetRequiredService<IActionDescriptorCollectionProvider>();
            if (actionDescriptorCollectionProvider != null)
            {
                var controllerActions = actionDescriptorCollectionProvider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>().ToList();
                if (!controllerActions.IsNullOrEmpty())
                {
                    foreach (var action in controllerActions)
                    {
                        var template = action.AttributeRouteInfo?.Template;
                        if (template != null)
                        {
                            template = template.TrimStart('/');
                            routes.Add(new OwinRoute(action.DisplayName,
                                $"/{template}"));
                        }

                        routes.Add(new OwinRoute(action.DisplayName,
                                                 $"/{action.ControllerName}/{action.ActionName}"));
                    }

                    owinPipeline.Routes.AddRange(routes);
                }

                var pagesActions = actionDescriptorCollectionProvider.ActionDescriptors.Items.OfType<PageActionDescriptor>().ToList();
                if (!pagesActions.IsNullOrEmpty())
                {
                    foreach (var action in pagesActions)
                    {
                        routes.Add(new OwinRoute(action.DisplayName,
                                                 action.ViewEnginePath,
                                                 action.RelativePath));
                    }

                    owinPipeline.Routes.AddRange(routes);
                }
            }
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }
    }

    public static bool IsUrlActionRoute(this IOwinPipeline owinPipeline, string url)
    {
        try
        {
            owinPipeline.Routes = owinPipeline.Routes ?? new List<OwinRoute>();
            var uri = new Uri(url);
            return owinPipeline.Routes.Any(x => x.RoutePath.Equals(uri.AbsolutePath, StringComparison.InvariantCultureIgnoreCase) ||
                                                x.RoutePath.Equals(uri.AbsolutePath + "/Index", StringComparison.InvariantCultureIgnoreCase));

        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }

        return false;
    }

    public static bool IsUrlErrorHandlingPath(this IOwinPipeline owinPipeline, string url)
    {
        try
        {
            var uri = new Uri(url);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return owinPipeline.ErrorHandlingPath.Equals(uri.AbsolutePath, StringComparison.InvariantCultureIgnoreCase) ||
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                   owinPipeline.ErrorHandlingPath.Equals(uri.AbsolutePath + "/Index", StringComparison.InvariantCultureIgnoreCase);

        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }

        return false;
    }
}