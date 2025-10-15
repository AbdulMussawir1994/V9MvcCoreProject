using V9MvcCoreProject.Entities.Models;

namespace V9MvcCoreProject.Extensions.LogsHelpers.Interface;

public interface ILogService
{
    Task<bool> InsertWebAppUserLogsAsync(UserActivityLogs ua);
}
