using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace V9MvcCoreProject.Middleware.PermissionAttribute;

public class CheckUserSession : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;

        if (!httpContext.Session.TryGetValue("UserPermissions", out _))
        {
            bool isAjaxRequest = httpContext.Request.Headers.TryGetValue("X-Requested-With", out var headerValue) &&
                                 string.Equals(headerValue, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);

            context.Result = isAjaxRequest
                ? new UnauthorizedResult()
                : new RedirectResult("~/Account/Login");

            return;
        }

        base.OnActionExecuting(context);
    }
}
