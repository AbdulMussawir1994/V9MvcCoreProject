using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace V9MvcCoreProject.Entities.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Cnic is required.")]
    [DisplayName("CNIC")]
    public string CNIC { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
