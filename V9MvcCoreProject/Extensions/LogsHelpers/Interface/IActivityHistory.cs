using V9MvcCoreProject.Entities.Models;

namespace V9MvcCoreProject.Extensions.LogsHelpers.Interface;

public interface IActivityHistory : IDisposable
{
    Task<bool> SaveActivityLogs(UserHistoryLogs history);
}
