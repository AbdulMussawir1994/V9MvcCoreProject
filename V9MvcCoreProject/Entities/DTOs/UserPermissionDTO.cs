namespace V9MvcCoreProject.Entities.DTOs;

public record struct UserPermissionDTO
{
    public int FormId { get; set; }
    public string UserId { get; set; }
    public List<int> FunctionId { get; set; }
    public bool FullAccess { get; set; }
}
