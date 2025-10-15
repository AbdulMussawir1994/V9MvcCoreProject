namespace V9MvcCoreProject.Entities.Models;

public class UserLoginLogs
{
    public long Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public DateTime LoginTime { get; set; }
    public bool LoginStatus { get; set; }
    public string StatusMessage { get; set; }
}
