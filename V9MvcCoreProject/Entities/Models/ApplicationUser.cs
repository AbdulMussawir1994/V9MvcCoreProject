using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace V9MvcCoreProject.Entities.Models;

public class ApplicationUser : IdentityUser<int>
{
    //public ApplicationUser()
    //{
    //    // Generate GUID when user is created
    //    Id = Guid.NewGuid().ToString();
    //}

    [Required]
    [StringLength(20)]
    public string CNIC { get; set; } = string.Empty;

    public int RoleTemplateId { get; set; }

    public string? ProfileImageUrl { get; set; }

    [StringLength(30)]
    public string? City { get; set; }

    [StringLength(30)]
    public string? State { get; set; }

    public string? CreatedBy { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    public virtual ICollection<Employee> Employees { get; private set; } = new List<Employee>();
}

public class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole() : base() { }

    public ApplicationRole(string roleName) : base(roleName)
    {
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }
}

//public class ApplicationRole : IdentityRole<string>
//{
//    public ApplicationRole()
//    {
//        Id = Guid.NewGuid().ToString();
//    }

//    public ApplicationRole(string roleName) : base(roleName)
//    {
//        Id = Guid.NewGuid().ToString();
//        ConcurrencyStamp = Guid.NewGuid().ToString();
//    }
//}
