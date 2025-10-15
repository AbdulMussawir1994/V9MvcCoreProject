namespace V9MvcCoreProject.Entities.Models;

public partial class PermissionTemplateDetail
{
    public int Id { get; set; }
    public int TemplateId { get; set; }
    public string FormName { get; set; }
    public int FunctionalityId { get; set; }
    public bool IsAllow { get; set; }

    public virtual PermissionTemplate Template { get; set; }
}