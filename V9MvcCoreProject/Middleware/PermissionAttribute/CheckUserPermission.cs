using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using V9MvcCoreProject.Entities.ViewModels;

namespace V9MvcCoreProject.Middleware.PermissionAttribute;

public class CheckUserPermission : ActionFilterAttribute
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CheckUserPermission> _logger;

    public CheckUserPermission(IMemoryCache cache, ILogger<CheckUserPermission> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var routeData = httpContext.GetRouteData().Values;

        string? controller = routeData["controller"]?.ToString();
        string? action = routeData["action"]?.ToString();

        // --- Validate session ---
        string? sessionPermissions = httpContext.Session.GetString("UserPermissions");
        if (string.IsNullOrEmpty(sessionPermissions))
        {
            context.Result = new RedirectResult("~/Account/Login");
            return;
        }

        // --- Try get cached permissions for this session ---
        string cacheKey = $"permissions-{httpContext.Session.Id}";
        if (!_cache.TryGetValue(cacheKey, out List<UserPermissionsViewModel>? permissions))
        {
            permissions = JsonConvert.DeserializeObject<List<UserPermissionsViewModel>>(sessionPermissions) ?? new();
            _cache.Set(cacheKey, permissions, TimeSpan.FromMinutes(10)); // cache for 10 minutes
        }

        // --- Check permission ---
        bool isAllowed = permissions.Any(p =>
            p.ActionMethodName.Equals(action, StringComparison.OrdinalIgnoreCase) &&
            (string.IsNullOrEmpty(p.ControllerName) || p.ControllerName.Equals(controller, StringComparison.OrdinalIgnoreCase))
        );

        bool isAjax = httpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        if (!isAllowed)
        {
            _logger.LogWarning("Unauthorized access by user. Controller={Controller}, Action={Action}, IsAjax={IsAjax}",
                controller, action, isAjax);

            context.Result = isAjax
                ? new UnauthorizedResult()
                : new RedirectResult("~/Home/Unauthorize");
            return;
        }

        base.OnActionExecuting(context);
    }
}