namespace V9MvcCoreProject.Entities.DTOs;

public readonly record struct PermissionTemplateDto
{
    public int Id { get; init; }
    public string TemplateName { get; init; }
}
