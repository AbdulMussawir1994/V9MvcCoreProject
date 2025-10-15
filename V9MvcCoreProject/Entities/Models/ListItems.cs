namespace V9MvcCoreProject.Entities.Models;

public partial class ListItems
{
    public int Id { get; set; }
    public int? ListTypeId { get; set; }
    public string Text { get; set; }
    public int? Value { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int? ModifiedBy { get; set; }
}