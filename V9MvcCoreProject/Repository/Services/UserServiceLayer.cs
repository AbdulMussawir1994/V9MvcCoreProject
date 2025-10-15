using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using V9MvcCoreProject.DataDbContext;
using V9MvcCoreProject.Entities.DTOs;
using V9MvcCoreProject.Entities.Models;
using V9MvcCoreProject.Entities.ViewModels;
using V9MvcCoreProject.Helpers;
using V9MvcCoreProject.Repository.Interface;

namespace V9MvcCoreProject.Repository.Services;

public class UserServiceLayer : IUserServiceLayer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;

    public UserServiceLayer(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                                        IConfiguration configuration, ApplicationDbContext db, RoleManager<ApplicationRole> roleManager)
    {
        _db = db;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _roleManager = roleManager;
    }

    public async Task<WebResponse<string>> UserLoginAsync(LoginViewModel model)
    {
        // ✅ Use a compiled query or AsNoTracking for performance
        var user = await _db.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.CNIC == model.CNIC);

        if (user is null)
            return WebResponse<string>.Failed("Please provide valid CNIC.");

        // ✅ Use UserManager for secure password verification
        var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!passwordValid)
            return WebResponse<string>.Failed("Password is not correct.");

        // ✅ No persistent cookie login, high performance lightweight
        var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: true);
        if (!signInResult.Succeeded)
            return WebResponse<string>.Failed("Account temporarily locked or login failed");

        // ✅ Generate secure JWT
        var token = GenerateJwtToken(user);

        return WebResponse<string>.Success(token, "Login successful");
    }

    public async Task<WebResponse<string>> UserRegisterAsync(RegisterViewModel model)
    {
        // 🧠 Use normalized values to avoid case-sensitive duplicates
        string normalizedEmail = model.Email.Trim().ToLowerInvariant();
        string normalizedUserName = model.Username.Trim().ToLowerInvariant();

        // ✅ Check for existing user by CNIC, Email, or Username
        var existingUser = await _db.Users.AsNoTracking()
            .Where(u => u.CNIC == model.CNIC
                     || u.Email.ToLower() == normalizedEmail
                     || u.UserName.ToLower() == normalizedUserName)
            .Select(u => new { u.CNIC, u.Email, u.UserName })
            .FirstOrDefaultAsync();

        if (existingUser != null)
        {
            string field = existingUser.CNIC == model.CNIC ? "CNIC" :
                           existingUser.Email.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase) ? "Email" :
                           "Username";

            return WebResponse<string>.Failed($"{field} already registered.");
        }

        // ✅ Create user entity
        var user = new ApplicationUser
        {
            UserName = normalizedUserName,
            Email = normalizedEmail,
            CNIC = model.CNIC.Trim(),
            IsActive = true,
            DateCreated = DateTime.UtcNow,
            LockoutEnabled = true,
            RoleTemplateId = 1 // Default role template
        };

        // ✅ Create and assign role
        return await CreateUserWithRoleAsync(user, model.Password, "User");
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<UserLoginResponseViewModel> CheckUserStatusAsync(string Cnic)
    {
        UserLoginResponseViewModel response = new UserLoginResponseViewModel();
        var user = await _db.Users.Where(x => x.CNIC == Cnic).FirstOrDefaultAsync().ConfigureAwait(false);
        if (user != null)
        {
            response.IsUserExists = true;
            if (user.IsActive)
            {
                response.Succeeded = true;
            }
            else
            {
                response.Succeeded = false;
                response.Error = $"User : {user.UserName} is inActive, Please Contact Admin";
            }

        }
        else
        {
            response.IsUserExists = false;
            response.Succeeded = false;
        }

        return response;
    }

    public async Task<UserLoginResponseViewModel> LoginAsync(string Cnic, string Password)
    {

        UserLoginResponseViewModel response = new UserLoginResponseViewModel();
        try
        {

            SignInResult result = new SignInResult();

            var user = await _db.Users.Where(x => x.CNIC == Cnic).FirstOrDefaultAsync().ConfigureAwait(false);
            if (user != null)
            {
                response.IsUserExists = true;
                if (user.PasswordHash == Password)
                {
                    response.Succeeded = true;
                }
                else
                {
                    result = await _signInManager.CheckPasswordSignInAsync(user, Password, false).ConfigureAwait(false);

                }
                if (result.Succeeded || response.Succeeded == true)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    response.UserId = user.Id;
                    response.Name = user.UserName;
                    response.Email = user.Email;
                    response.RoleTemplateId = user.RoleTemplateId;
                    response.Succeeded = true;
                }
                else
                {
                    response.Succeeded = false;
                }
            }
            else
            {
                response.IsUserExists = false;
                response.Succeeded = false;
            }

            return response;
        }
        catch (Exception)
        {
            return response;
        }
    }

    public async Task<ActionResponseDto> LoginAttemptLog(UserLoginLogs userLoginLog)
    {
        ActionResponseDto actionResponse = new ActionResponseDto();
        _db.UserLoginLogs.Add(userLoginLog);
        await _db.SaveChangesAsync();
        actionResponse.ErrorMessage = "Saved Successfully";
        actionResponse.Success = true;
        return actionResponse;
    }


    #region Private Methods
    private string GenerateJwtToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"] ?? "0");

        var claims = new List<Claim>
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // ✅ Optional: Include roles for authorization
        var roles = _userManager.GetRolesAsync(user).Result;
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<string> CheckDuplicateUserAsyncIQ(string email, string cnic, string username)
    {
        // Normalize email once (better performance)
        var normalizedEmail = email?.Trim().ToLowerInvariant();

        // Build IQueryable dynamically (no premature ToList/Async)
        IQueryable<ApplicationUser> query = _db.Users.AsNoTracking();

        query = query.Where(u => u.Email.ToLower() == normalizedEmail || u.CNIC == cnic || u.UserName == username);

        // Still IQueryable at this point - SQL gets generated here
        var existing = await query
            .Select(u => new { u.Email, u.CNIC, u.UserName })
            .FirstOrDefaultAsync();

        if (existing is null)
            return string.Empty;

        if (normalizedEmail != null && string.Equals(existing.Email, normalizedEmail, StringComparison.OrdinalIgnoreCase))
            return "Email";

        if (cnic != null && existing.CNIC == cnic)
            return "CNIC";

        if (username != null && existing.UserName == username)
            return "Username";

        return string.Empty;
    }

    private async Task<WebResponse<string>> CreateUserWithRoleAsync(ApplicationUser user, string password, string defaultRole)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync();

        try
        {
            // 🧩 Create user with hashed password
            var createResult = await _userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                await transaction.RollbackAsync();
                return WebResponse<string>.Failed(errors);
            }

            // 🛡️ Ensure role exists
            if (!await _roleManager.RoleExistsAsync(defaultRole))
            {
                var roleResult = await _roleManager.CreateAsync(new ApplicationRole(defaultRole));
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                    await transaction.RollbackAsync();
                    return WebResponse<string>.Failed(errors);
                }
            }

            // 👤 Assign user role
            var roleAssignResult = await _userManager.AddToRoleAsync(user, defaultRole);
            if (!roleAssignResult.Succeeded)
            {
                var errors = string.Join("; ", roleAssignResult.Errors.Select(e => e.Description));
                await transaction.RollbackAsync();
                return WebResponse<string>.Failed(errors);
            }

            await transaction.CommitAsync();
            return WebResponse<string>.Success("User created successfully.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return WebResponse<string>.Failed("An unexpected error occurred while creating the user.");
        }
    }

    #endregion
}