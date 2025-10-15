using V9MvcCoreProject.Entities.DTOs;
using V9MvcCoreProject.Entities.Models;
using V9MvcCoreProject.Entities.ViewModels;

namespace V9MvcCoreProject.Repository.Interface;


public interface IUserAccessServiceLayer
{
    List<ApplicationUser> GetAllUsersAsync();
    Task<bool> ChangePermissionAsync(UserPermissionDTO dto);
    Task<List<UserPermissionsViewModel>> GetUserPermissionsAsync(string userid);
    Task<List<UserPermissionsViewModel>> GetUserPermissionsByRoleIdAsync(int RoleId);
    Task<List<Items>> GetMenu(int RoleId);
}
