using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace V9MvcCoreProject.Entities.Models;

public class ApplicationUser : IdentityUser<int>
{
    [Required]
    [StringLength(20)]
    public string CNIC { get; set; }
    public int RoleTemplateId { get; set; }
    public string? ProfileImageUrl { get; set; }
    [StringLength(30)]
    public string? City { get; set; }
    [StringLength(30)]
    public string? State { get; set; }

    public long? CreatedBy { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public long? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;


    public virtual ICollection<Employee> Employees { get; private set; } = new List<Employee>();
}

public class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole() : base()
    {
    }

    public ApplicationRole(string roleName) : base()
    {
        Name = roleName;
        NormalizedName = roleName.ToUpperInvariant();
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }
}
