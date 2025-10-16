using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace V9MvcCoreProject.Middleware.PermissionAttribute;

public sealed class CheckUserSession : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;

        // 🔹 1. Fast null check — avoids allocating byte array from Session.TryGetValue unnecessarily
        if (httpContext.Session == null || !httpContext.Session.Keys.Contains("UserPermissions"))
        {
            HandleInvalidSession(context);
            return;
        }

        // 🔹 2. (Optional) quick user check — ensures identity matches session (useful for SSO / concurrent session validation)
        if (!httpContext.User.Identity?.IsAuthenticated ?? true)
        {
            HandleInvalidSession(context);
            return;
        }

        base.OnActionExecuting(context);
    }

    private void HandleInvalidSession(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        bool isAjaxRequest = httpContext.Request.Headers.TryGetValue("X-Requested-With", out var header) &&
                             string.Equals(header, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);

        //_logger.LogWarning("Session expired or invalid. Path={Path}, IsAjax={IsAjax}", httpContext.Request.Path, isAjaxRequest);

        context.Result = isAjaxRequest
            ? new UnauthorizedResult()
            : new RedirectResult("~/User/Login");
    }
}
