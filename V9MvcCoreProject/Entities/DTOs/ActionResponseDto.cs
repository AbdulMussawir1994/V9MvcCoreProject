namespace V9MvcCoreProject.Entities.DTOs;

public record struct ActionResponseDto
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public int Id { get; set; }
}
