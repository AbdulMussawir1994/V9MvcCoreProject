namespace V9MvcCoreProject.Entities.Models;

public partial class PermissionTemplate
{
    public PermissionTemplate()
    {
        PermissionTemplateDetail = new HashSet<PermissionTemplateDetail>();
    }

    public int Id { get; set; }
    public string TemplateName { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<PermissionTemplateDetail> PermissionTemplateDetail { get; set; }
}
