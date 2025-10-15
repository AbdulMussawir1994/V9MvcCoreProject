using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using V9MvcCoreProject.Entities.Models;
using V9MvcCoreProject.Entities.ViewModels;
using V9MvcCoreProject.Extensions.LogsHelpers.Interface;
using V9MvcCoreProject.Repository.Interface;

namespace V9MvcCoreProject.Controllers;

public class AccountController : Controller
{
    private readonly IUserServiceLayer _userService;
    private readonly IUserAccessServiceLayer _userAccess;
    private readonly IConfiguration _config;
    private readonly ILoggerManager _log;

    public AccountController(IUserServiceLayer userService, IUserAccessServiceLayer userAccess, IConfiguration config, ILoggerManager logger)
    {
        _log = logger;
        _config = config;
        _userAccess = userAccess;
        _userService = userService;


    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        var response = new LoginViewModel();
        return View(response);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<JsonResult> Login(LoginViewModel user, string ReturnUrl)
    {
        try
        {
            UserLoginLogs userLoginLog = new UserLoginLogs();
            LoginResponseViewModel loginResponse = new LoginResponseViewModel();
            var IsActiveResponse = await _userService.CheckUserStatusAsync(user.CNIC);
            if (IsActiveResponse.Succeeded)
            {
                var result = await _userService.LoginAsync(user.CNIC, user.Password);
                userLoginLog.UserId = result.UserId;
                userLoginLog.LoginTime = DateTime.Now;
                userLoginLog.UserName = result.Email;
                if (result.Succeeded && result.IsUserExists)
                {
                    var defaultPassword = _config["EncryptionSettings:DefaultPassword"].ToString();
                    if (user.Password.ToLower() == defaultPassword.ToLower())
                    {
                        HttpContext.Session.SetString("ResetDefPass", "DefaultPassword");
                    }
                    userLoginLog.LoginStatus = true;
                    userLoginLog.StatusMessage = loginResponse.Success;
                    await _userService.LoginAttemptLog(userLoginLog);
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim("Name", result.Name));
                    // identity.AddClaim(new Claim("Role", result.Result.UserRole));
                    identity.AddClaim(new Claim("UserId", result.UserId.ToString()));
                    identity.AddClaim(new Claim("RoleId", result.RoleTemplateId.ToString()));
                    //List<UserPermissionsModel> permissions = _useraccess.GetUserPermissionsAsync(result.UserId.ToString());

                    //permission changed to role permissions -- Sohaib
                    List<UserPermissionsViewModel> permissions = await _userAccess.GetUserPermissionsByRoleIdAsync(result.RoleTemplateId);
                    var serializedpermissions = JsonConvert.SerializeObject(permissions);
                    //Dynamic Menu
                    var GetMenu = await _userAccess.GetMenu(result.RoleTemplateId);
                    var SerializedMenu = JsonConvert.SerializeObject(GetMenu);
                    // identity.AddClaim(new Claim("UserPermissions", serializedpermissions));
                    HttpContext.Session.SetString("UserPermissions", serializedpermissions);
                    HttpContext.Session.SetString("userEmail", result.Email);
                    HttpContext.Session.SetString("DynamicMenu", SerializedMenu);
                    HttpContext.Session.SetString("UserName", result.Name);

                    //var test= HttpContext.Session.GetString("UserPermissions");
                    // Authenticate using the identity
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = false });
                }
                else if (!result.IsUserExists)
                {
                    userLoginLog.LoginStatus = false;
                    userLoginLog.StatusMessage = loginResponse.NoUserExists;
                    await _userService.LoginAttemptLog(userLoginLog);
                }
                else if (!result.Succeeded)
                {
                    userLoginLog.LoginStatus = false;
                    userLoginLog.StatusMessage = loginResponse.IncorrectDetails;
                    await _userService.LoginAttemptLog(userLoginLog);
                }

                _log.LogInformation("Test");

                // string Role = HttpContext.User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value;
                return Json(result);
            }
            else
            {
                userLoginLog.UserId = 0;
                userLoginLog.LoginTime = DateTime.Now;
                userLoginLog.UserName = user.CNIC;
                userLoginLog.LoginStatus = false;
                userLoginLog.StatusMessage = loginResponse.Failed;
                await _userService.LoginAttemptLog(userLoginLog);
                return Json(IsActiveResponse);
            }
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);
            throw;
        }
    }

    [HttpGet]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> UserLoginAsync([FromForm] LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                IsSuccess = false,
                Message = "Invalid data provided. Please check your input.",
                Code = "400"
            });
        }

        try
        {
            var response = await _userService.UserLoginAsync(model);

            if (response is null)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Unexpected error. Please try again later.",
                    Code = "400"
                });
            }

            if (!response.IsSuccess)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = response.Message ?? "Unfound Error",
                    Code = "400"
                });
            }

            // ✅ Login success - store session or token if needed
            HttpContext.Session.SetString("AuthToken", response.Value?.ToString() ?? "");
            HttpContext.Session.SetString("Username", model.CNIC ?? "");

            return Json(new
            {
                IsSuccess = true,
                Message = "Login successful!",
                Code = "200"
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                IsSuccess = false,
                Message = "Internal server error: " + ex.Message,
                Code = "500"
            });
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new
            {
                IsSuccess = false,
                Message = "Invalid data provided. Please check your input.",
                Code = "400"
            });
        }

        try
        {
            var response = await _userService.UserRegisterAsync(model);

            if (response is null)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Unexpected error. Please try again later.",
                    Code = "400"
                });
            }

            if (!response.IsSuccess)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = response.Message ?? "Unfound Error",
                    Code = "400"
                });
            }

            return Json(new
            {
                IsSuccess = true,
                Message = "Registration successful! Please log in.",
                Code = "200"
            });

        }
        catch
        (Exception ex)
        {
            return Json(new
            {
                IsSuccess = false,
                Message = "Internal server error: " + ex.Message,
                Code = "500"
            });
        }
    }

    [HttpPost]
    //[AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOut()
    {
        HttpContext.Session.Clear();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }

}
