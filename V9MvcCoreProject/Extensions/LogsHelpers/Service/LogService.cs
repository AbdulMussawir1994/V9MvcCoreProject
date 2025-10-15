using V9MvcCoreProject.DataDbContext;
using V9MvcCoreProject.Entities.Models;
using V9MvcCoreProject.Extensions.LogsHelpers.Interface;

namespace V9MvcCoreProject.Extensions.LogsHelpers.Service;

public class LogService : ILogService
{
    private readonly ApplicationDbContext _ctx;
    private readonly ILogger<LogService> _logger;

    public LogService(ApplicationDbContext dbContext, ILogger<LogService> logger)
    {
        _ctx = dbContext;
        _logger = logger;
    }

    public async Task<bool> InsertWebAppUserLogsAsync(UserActivityLogs ua)
    {
        try
        {
            _ctx.ChangeTracker.AutoDetectChangesEnabled = false;
            await _ctx.UserActivityLogs.AddAsync(ua);
            await _ctx.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inserting user activity log.");
            return false;
        }
    }
}
