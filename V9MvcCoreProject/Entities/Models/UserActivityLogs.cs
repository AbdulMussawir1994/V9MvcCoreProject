namespace V9MvcCoreProject.Entities.Models;

public partial class UserActivityLogs
{
    public int Id { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }
    public string Path { get; set; }
    public string Method { get; set; }
    public string QueryString { get; set; }
    public string RequestBody { get; set; }
    public string ResponseBody { get; set; }
    public long? UserId { get; set; }
    public DateTime Datetime { get; set; }
    public bool IsException { get; set; }
    public string Exception { get; set; }
    public bool IsAjaxRequest { get; set; }
    public string LogId { get; set; }
}
