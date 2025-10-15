namespace V9MvcCoreProject.Entities.DTOs;

public record struct GetUserFunctionalitiesDTO
{
    public string UserId { get; set; }
    public int FormId { get; set; }
}
