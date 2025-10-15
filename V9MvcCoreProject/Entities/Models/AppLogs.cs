namespace V9MvcCoreProject.Entities.Models;

public partial class AppLogs
{
    public string Application { get; set; }
    public DateTime? Logged { get; set; }
    public string Level { get; set; }
    public string Message { get; set; }
    public string Logger { get; set; }
    public string Callsite { get; set; }
    public string Exception { get; set; }
    public string UserId { get; set; }
    public string Email { get; set; }
    public string DeviceId { get; set; }
    public string Ip { get; set; }
    public string FunctionName { get; set; }
    public string ControllerName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }
    public string RequestParameters { get; set; }
    public string RequestResponse { get; set; }
    public string Channel { get; set; }
    public string LogId { get; set; }
    public DateTime? RequestDateTime { get; set; }
    public string ErrorCode { get; set; }
    public long Id { get; set; }
    public string RequestId { get; set; }
}
