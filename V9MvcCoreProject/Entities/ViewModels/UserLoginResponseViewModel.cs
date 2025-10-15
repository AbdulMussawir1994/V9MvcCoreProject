namespace V9MvcCoreProject.Entities.ViewModels;

public class UserLoginResponseViewModel
{
    public bool Succeeded { get; set; }
    public bool IsUserExists { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int UserId { get; set; }
    public string UserRole { get; set; }
    public string Error { get; set; }
    public int RoleTemplateId { get; set; }

}
