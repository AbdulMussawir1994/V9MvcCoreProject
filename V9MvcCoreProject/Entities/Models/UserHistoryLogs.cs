namespace V9MvcCoreProject.Entities.Models;

public class UserHistoryLogs
{
    public long Id { get; set; }
    public string LogId { get; set; }
    public string Action { get; set; }
    public string ActionMethod { get; set; }
    public string NewValueJson { get; set; }
    public string OldValueJson { get; set; }
    public long? UserId { get; set; }
    public DateTime CreatedDate { get; set; }
}
