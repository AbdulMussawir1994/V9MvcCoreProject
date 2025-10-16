using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceStack.Text;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using V9MvcCoreProject.Entities.Models;
using V9MvcCoreProject.Extensions.LogsHelpers.Interface;

namespace V9MvcCoreProject.Middleware;

public class LogsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IActionResultExecutor<ObjectResult> _executor;
    private readonly RecyclableMemoryStreamManager _streamManager = new();
    private readonly IConfiguration _config;
    private readonly ILogger<LogsMiddleware> _logger;

    public LogsMiddleware(RequestDelegate next, IConfiguration config,
                          IActionResultExecutor<ObjectResult> executor,
                          ILogger<LogsMiddleware> logger)
    {
        _next = next;
        _config = config;
        _executor = executor;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ILogService log)
    {
        var logId = Guid.NewGuid().ToString();
        context.Items["LogId"] = logId; // better than Session (stateless, thread-safe)

        string reqBody = string.Empty;
        string resBody = string.Empty;
        long userId = GetUserId(context);

        var routeData = context.GetRouteData();
        string controller = routeData.Values["controller"]?.ToString();
        string action = routeData.Values["action"]?.ToString();
        bool isAjax = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        try
        {
            reqBody = await ReadRequestBodyAsync(context);
            reqBody = MaskSensitiveData(reqBody);

            var originalBody = context.Response.Body;
            await using var newBody = _streamManager.GetStream();
            context.Response.Body = newBody;

            await _next(context); // Execute request pipeline

            resBody = await ReadResponseBodyAsync(context);
            resBody = MaskSensitiveData(resBody);

            await newBody.CopyToAsync(originalBody);
        }
        catch (Exception ex)
        {
            resBody = $"Exception: {ex.Message}";
            _logger.LogError(ex, "Error in LogsMiddleware");
        }

        var logModel = new UserActivityLogs
        {
            LogId = logId,
            Method = context.Request.Method,
            Path = context.Request.Path,
            QueryString = context.Request.QueryString.ToString(),
            UserId = userId,
            Action = action,
            RequestBody = reqBody,
            ResponseBody = resBody,
            Datetime = DateTime.UtcNow,
            Controller = controller,
            IsAjaxRequest = isAjax,
            IsException = (reqBody?.Contains("exception", StringComparison.OrdinalIgnoreCase) ?? false) || (resBody?.Contains("exception", StringComparison.OrdinalIgnoreCase) ?? false),
            Exception = (reqBody?.Contains("exception", StringComparison.OrdinalIgnoreCase) == true) ? reqBody : (resBody?.Contains("exception", StringComparison.OrdinalIgnoreCase) == true) ? resBody : string.Empty
        };

        _ = Task.Run(async () =>
        {
            try
            {
                using var scope = context.RequestServices.CreateScope();
                var scopedLog = scope.ServiceProvider.GetRequiredService<ILogService>();
                await scopedLog.InsertWebAppUserLogsAsync(logModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 Background logging failed: {ex.Message}");
            }
        });
    }

    #region Helper Methods

    private static long GetUserId(HttpContext context)
    {
        if (context.User.Identity is ClaimsIdentity identity)
        {
            var claim = identity.FindFirst("UserId");
            if (long.TryParse(claim?.Value, out var id))
                return id;
        }
        return 0;
    }

    private async Task<string> ReadRequestBodyAsync(HttpContext context)
    {
        context.Request.EnableBuffering();

        using var stream = _streamManager.GetStream();
        await context.Request.Body.CopyToAsync(stream);
        context.Request.Body.Position = 0;

        string body = await StreamToStringAsync(stream);
        return string.IsNullOrWhiteSpace(body) ? "PAGE GET REQUEST" : body;
    }

    private async Task<string> ReadResponseBodyAsync(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        string text = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        return text.Contains("DOCTYPE") ? "PAGE HTML RESPONSE" : text;
    }

    private static async Task<string> StreamToStringAsync(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(stream);
        string val = await reader.ReadToEndAsync();
        return HttpUtility.UrlDecode(val);
    }

    private static string MaskSensitiveData(string data)
    {
        if (string.IsNullOrEmpty(data)) return data;

        try
        {
            var token = JToken.Parse(data);
            MaskJsonFields(token);
            return token.ToString(Formatting.None);
        }
        catch
        {
            // Fallback to regex-based masking
            data = Regex.Replace(data, @"\b\d{12,19}\b", "****"); // Credit card
            data = Regex.Replace(data, @"(Email|Password|Token|AuthKey)=([^&\s]+)", "$1=****", RegexOptions.IgnoreCase);
            return data;
        }
    }

    private static void MaskJsonFields(JToken token)
    {
        if (token.Type == JTokenType.Object)
        {
            foreach (var prop in token.Children<JProperty>())
            {
                if (Regex.IsMatch(prop.Name, "password|token|secret|key|confirmpassword", RegexOptions.IgnoreCase))
                    prop.Value = "****";
                else
                    MaskJsonFields(prop.Value);
            }
        }
        else if (token.Type == JTokenType.Array)
        {
            foreach (var item in token.Children())
                MaskJsonFields(item);
        }
    }
    #endregion
}
