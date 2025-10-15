using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using V9MvcCoreProject.Entities.ViewModels;

namespace V9MvcCoreProject.Middleware.PermissionAttribute;

public class CheckUserPermission : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var session = httpContext.Session;

        // Get user permissions directly from session
        var permissionsJson = session.GetString("UserPermissions");
        if (string.IsNullOrEmpty(permissionsJson))
        {
            context.Result = new RedirectResult("~/Account/Login");
            return;
        }

        // Quickly deserialize (safe check)
        if (!TryDeserialize(permissionsJson, out List<UserPermissionsViewModel>? permissions))
        {
            context.Result = new RedirectResult("~/Account/Login");
            return;
        }

        // Get route data
        var routeValues = httpContext.GetRouteData().Values;
        string action = routeValues["action"]?.ToString() ?? string.Empty;
        string controller = routeValues["controller"]?.ToString() ?? string.Empty;

        // Match permission
        bool isAllowed = permissions.Any(p =>
            string.Equals(p.ActionMethodName, action, StringComparison.OrdinalIgnoreCase) &&
            (string.IsNullOrEmpty(p.ControllerName) || string.Equals(p.ControllerName, controller, StringComparison.OrdinalIgnoreCase))
        );

        bool isAjax = IsAjaxRequest(httpContext.Request);
        //    bool isAjax = AjaxRequest.IsAjaxRequest(httpContext.Request);

        if (!isAllowed)
        {
            context.Result = isAjax
                ? new UnauthorizedResult()
                : new RedirectResult("~/Home/Unauthorize");
            return;
        }

        base.OnActionExecuting(context);
    }

    private static bool TryDeserialize(string json, out List<UserPermissionsViewModel>? list)
    {
        try
        {
            list = JsonConvert.DeserializeObject<List<UserPermissionsViewModel>>(json);
            return list != null;
        }
        catch
        {
            list = null;
            return false;
        }
    }

    private static bool IsAjaxRequest(HttpRequest request)
    {
        return request.Headers.TryGetValue("X-Requested-With", out var value)
               && string.Equals(value, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
    }
}