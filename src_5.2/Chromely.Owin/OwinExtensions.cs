// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely.Owin;

public static class OwinExtensions
{
    /// <summary>
    /// Checks if the application is of OWIN type.
    /// </summary>
    /// <param name="app"></param>
    /// <returns>true if of OWIN type, otherwise false.</returns>
    public static bool IsOwinApp(this ChromelyApp app)
    {
        return typeof(IOwinAppStartup).IsAssignableFrom(app.GetType());
    }

    /// <summary>
    /// Checks of error code is a client error code (4xx).
    /// </summary>
    /// <param name="statusCode">The status code - type of <see cref="HttpStatusCode"/>.</param>
    /// <returns>true if error code is client error code, otherwise false.</returns>
    public static bool IsClientErrorCode(this HttpStatusCode statusCode)
    {
        // 4xx client errors
        return (int)statusCode >= ResponseConstants.MinClientErrorStatusCode &&
               (int)statusCode <= ResponseConstants.MaxClientErrorStatusCode;
    }

    /// <summary>
    /// Checks of server code is a client error code (4xx).
    /// </summary>
    /// <param name="statusCode">The status code - type of <see cref="HttpStatusCode"/>.</param>
    /// <returns>true if error code is client server code, otherwise false.</returns>
    public static bool IsServerErrorCode(this HttpStatusCode statusCode)
    {
        // 5xx server errors
        return (int)statusCode >= ResponseConstants.MinServerErrorStatusCode;
    }

    /// <summary>
    /// Gets the mime type from dictionary of headers.
    /// </summary>
    /// <param name="headers">The headers.</param>
    /// <returns>the mime type.</returns>
    public static string GetMimeType(this IDictionary<string, string[]> headers)
    {
       if (headers is null || !headers.Any())
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

    /// <summary>
    /// Parse routes and add to the pipeline - instance of <see cref="IOwinPipeline"/>.
    /// </summary>
    /// <param name="owinPipeline">Instance of <see cref="IOwinPipeline"/>.</param>
    /// <param name="provider">The <see cref="IServiceProvider"/> for application services.</param>
    public static void ParseRoutes(this IOwinPipeline owinPipeline, IServiceProvider provider)
    {
        try
        {
            owinPipeline.Routes ??= new List<OwinRoute>();
            var routes = new List<OwinRoute>();

            var actionDescriptorCollectionProvider = provider.GetRequiredService<IActionDescriptorCollectionProvider>();
            if (actionDescriptorCollectionProvider is not null)
            {
                var controllerActions = actionDescriptorCollectionProvider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>().ToList();
                if (!controllerActions.IsNullOrEmpty())
                {
                    foreach (var action in controllerActions)
                    {
                        var template = action.AttributeRouteInfo?.Template;
                        if (template is not null)
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

    /// <summary>
    /// Checks if the specified url is an OWIN action route.
    /// </summary>
    /// <param name="owinPipeline"></param>
    /// <param name="url"></param>
    /// <returns>true if an OWIN action route, otherwise false.</returns>
    public static bool IsUrlActionRoute(this IOwinPipeline owinPipeline, string url)
    {
        try
        {
            owinPipeline.Routes ??= new List<OwinRoute>();
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

    /// <summary>
    /// Checks if the specified url is an OWIN error handling route.
    /// </summary>
    /// <param name="owinPipeline"></param>
    /// <param name="url"></param>
    /// <returns>true if an OWIN error handling route, otherwise false.</returns>
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