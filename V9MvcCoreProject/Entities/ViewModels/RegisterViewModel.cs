using System.ComponentModel.DataAnnotations;

namespace V9MvcCoreProject.Entities.ViewModels;


public class RegisterViewModel
{
    [Required(ErrorMessage = "CNIC is required.")]
    public string CNIC { get; set; }

    [Display(Name = "Username")]
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }

    [EmailAddress]
    [Display(Name = "Email address")]
    [Required(ErrorMessage = "Email address is required")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Confirm Password")]
    [Required(ErrorMessage = "Confirm password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password do not match")]
    public string ConfirmPassword { get; set; }
}
