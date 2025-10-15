namespace V9MvcCoreProject.Entities.Models;

public partial class ApplicationFunctionalities
{
    public int Id { get; set; }
    public string FunctionalityName { get; set; }
    public int? FormId { get; set; }
    public bool? IsActive { get; set; }

    public string ActionMethodName { get; set; }
    public bool? IsMenuItem { get; set; }
    public string MenuReferenceName { get; set; }
}
