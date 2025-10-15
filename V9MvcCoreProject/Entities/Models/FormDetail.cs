namespace V9MvcCoreProject.Entities.Models;

public partial class FormDetail
{
    public int Id { get; set; }
    public string ControllerName { get; set; }
    public string ActionName { get; set; }
    public string FormName { get; set; }
    public bool? IsActive { get; set; }
    public string DisplayName { get; set; }
    public string? IconCode { get; set; }
    public int? DisplayOrder { get; set; }
}
