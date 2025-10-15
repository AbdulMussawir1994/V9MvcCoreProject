using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace V9MvcCoreProject.Entities.Models;

public partial class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [Precision(18, 2)]
    public decimal Salary { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public string ApplicationUserId { get; set; }

    [ForeignKey(nameof(ApplicationUserId))]
    public virtual ApplicationUser AppUser { get; set; } = null!;
}
