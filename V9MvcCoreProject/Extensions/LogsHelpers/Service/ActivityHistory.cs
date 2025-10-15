using V9MvcCoreProject.DataDbContext;
using V9MvcCoreProject.Entities.Models;
using V9MvcCoreProject.Extensions.LogsHelpers.Interface;

namespace V9MvcCoreProject.Extensions.LogsHelpers.Service;

public class ActivityHistory : IActivityHistory
{
    private bool alreadyDisposed = false;
    private readonly ApplicationDbContext ctx;
    public ActivityHistory(ApplicationDbContext dbcontext)
    {
        this.ctx = dbcontext;
    }

    public async Task<bool> SaveActivityLogs(UserHistoryLogs history)
    {
        try
        {
            history.CreatedDate = DateTime.Now;

            ctx.UserHistoryLogs.Add(history);
            await ctx.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;

        }
    }

    #region "Dispose"
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool isDisposing)
    {
        if (alreadyDisposed)
            return;
        alreadyDisposed = true;
    }

    #endregion
}
