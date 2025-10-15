using V9MvcCoreProject.Entities.DTOs;
using V9MvcCoreProject.Entities.Models;
using V9MvcCoreProject.Entities.ViewModels;
using V9MvcCoreProject.Helpers;

namespace V9MvcCoreProject.Repository.Interface;

public interface IUserServiceLayer
{
    Task SignOutAsync();
    Task<WebResponse<string>> UserLoginAsync(LoginViewModel loginViewModel);
    Task<WebResponse<string>> UserRegisterAsync(RegisterViewModel model);
    Task<UserLoginResponseViewModel> CheckUserStatusAsync(string Email);
    Task<UserLoginResponseViewModel> LoginAsync(string Email, string Password);
    Task<ActionResponseDto> LoginAttemptLog(UserLoginLogs userLoginLog);
}
