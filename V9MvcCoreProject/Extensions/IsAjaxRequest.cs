namespace V9MvcCoreProject.Extensions;

public class AjaxRequest
{
    public static bool IsAjaxRequest(HttpRequest request)
    {
        if (request == null)
            throw new ArgumentNullException("request");

        if (request.Headers != null)
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        return false;
    }
}