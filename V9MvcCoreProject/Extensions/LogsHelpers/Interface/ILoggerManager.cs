namespace V9MvcCoreProject.Extensions.LogsHelpers.Interface;

public interface ILoggerManager
{
    void LogInformation(string msg);
    void LogError(string msg, Exception exception);
}
