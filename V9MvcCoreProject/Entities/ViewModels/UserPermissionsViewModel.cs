namespace V9MvcCoreProject.Entities.ViewModels;

public class UserPermissionsViewModel
{
    public int formId { get; set; }
    public string FunctionalityName { get; set; }
    public bool? FullAccess { get; set; }
    public bool? AllowAccess { get; set; }
    public string FormName { get; set; }
    public string ActionName { get; set; }
    public string ControllerName { get; set; }
    public string ActionMethodName { get; set; }
}

public class Permission
{
    public string FunctionalityName { get; set; }
    public int FunctionalityId { get; set; }
    public bool? IsSelected { get; set; }
}

public class AppFunctionality
{
    public int Id { get; set; }
    public string FunctionalityName { get; set; }
    public bool IsAllow { get; set; }
}

public class CustomUserAccess
{
    public int? FunctionalityId { get; set; }
    public string FunctionalityName { get; set; }
    public bool? AllowAccess { get; set; }
    public bool? IsFullAccess { get; set; }
}

public class PermissionResponse
{
    public List<Permission> list { get; set; }
    public bool? IsFullAccess { get; set; }
}
