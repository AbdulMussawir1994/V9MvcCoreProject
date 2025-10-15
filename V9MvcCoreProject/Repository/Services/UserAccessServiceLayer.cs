using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using V9MvcCoreProject.DataDbContext;
using V9MvcCoreProject.Entities.DTOs;
using V9MvcCoreProject.Entities.Models;
using V9MvcCoreProject.Entities.ViewModels;
using V9MvcCoreProject.Repository.Interface;

namespace V9MvcCoreProject.Repository.Services;

public class UserAccessServiceLayer : IUserAccessServiceLayer
{
    private bool alreadyDisposed = false;
    public ApplicationDbContext _ctx;
    private readonly UserManager<ApplicationUser> _um;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool isDisposing)
    {
        // Don't dispose more than once.
        if (alreadyDisposed)
            return;

        //if (isDisposing)
        //{
        //    // elided: free managed resources here.
        //    if (dsReport != null)
        //    {
        //        dsReport.Dispose();
        //        dsReport = null;
        //    }
        //}

        // elided: free unmanaged resources here.
        // Set disposed flag:
        alreadyDisposed = true;
    }
    public UserAccessServiceLayer(UserManager<ApplicationUser> um, ApplicationDbContext ctx)
    {
        _ctx = ctx;
        _um = um;
    }

    public List<ApplicationUser> GetAllUsersAsync()
    {
        List<ApplicationUser> users = new List<ApplicationUser>();
        users = _um.Users.ToList();
        return users;
    }

    public async Task<bool> ChangePermissionAsync(UserPermissionDTO model)
    {
        string FormName;
        List<UserAccess> accesslist = new List<UserAccess>();

        FormName = await _ctx.FormDetail.Where(x => x.Id == model.FormId).Select(x => x.FormName).SingleOrDefaultAsync();
        _ctx.UserAccess.RemoveRange(_ctx.UserAccess.Where(x => x.UserId == model.UserId && x.FormName == FormName));
        await _ctx.SaveChangesAsync();
        if (model.FunctionId == null && model.FullAccess == false)
        {
            return true;
        }
        else
        {
            if (model.FunctionId == null && model.FullAccess)
            {
                accesslist.Add(new UserAccess { AllowAccess = true, ApplicationFunctionalityId = 0, FormName = FormName, FullAccess = model.FullAccess, UserId = model.UserId });
                _ctx.UserAccess.AddRange(accesslist);
                await _ctx.SaveChangesAsync();
            }
            else
            {
                if (model.FunctionId.Count > 0 || model.FullAccess)
                {
                    if (model.FunctionId.Count == 0 && model.FullAccess)
                    {
                        accesslist.Add(new UserAccess { AllowAccess = false, ApplicationFunctionalityId = 0, FormName = FormName, FullAccess = model.FullAccess, UserId = model.UserId });
                    }
                    else
                    {
                        foreach (int id in model.FunctionId)
                        {
                            accesslist.Add(new UserAccess { AllowAccess = true, ApplicationFunctionalityId = id, FormName = FormName, FullAccess = model.FullAccess, UserId = model.UserId });
                        }
                    }
                }
                _ctx.UserAccess.AddRange(accesslist);
                await _ctx.SaveChangesAsync();
            }
        }
        return true;
    }

    public async Task<List<UserPermissionsViewModel>> GetUserPermissionsAsync(string userid)
    {
        List<UserPermissionsViewModel> permissions = new List<UserPermissionsViewModel>();
        permissions = await (from ua in _ctx.UserAccess
                             join af in _ctx.ApplicationFunctionalities on ua.ApplicationFunctionalityId equals af.Id into ps
                             from af in ps.DefaultIfEmpty()
                             where ua.UserId == userid
                             select new UserPermissionsViewModel
                             {
                                 FunctionalityName = af.FunctionalityName,
                                 FullAccess = ua.FullAccess,
                                 AllowAccess = ua.AllowAccess,
                                 FormName = ua.FormName,
                                 ActionMethodName = af.ActionMethodName
                             }
                       ).ToListAsync();
        return permissions;
    }
    public async Task<List<UserPermissionsViewModel>> GetUserPermissionsByRoleIdAsync(int RoleId)
    {
        List<UserPermissionsViewModel> permissions = new List<UserPermissionsViewModel>();
        permissions = await (from ua in _ctx.PermissionTemplateDetail
                             join af in _ctx.ApplicationFunctionalities on ua.FunctionalityId equals af.Id into ps
                             from af in ps.DefaultIfEmpty()
                             where ua.TemplateId == RoleId
                             select new UserPermissionsViewModel
                             {
                                 formId = (int)af.FormId,
                                 FunctionalityName = af.FunctionalityName,
                                 FullAccess = false,
                                 AllowAccess = ua.IsAllow,
                                 FormName = ua.FormName,
                                 ActionMethodName = af.ActionMethodName
                             }).ToListAsync();
        return permissions;
    }
    public async Task<List<Items>> GetMenu(int RoleId)
    {
        List<Items> items = new List<Items>() { };
        var FormDetails = await _ctx.FormDetail.ToListAsync();

        items = await (from f1 in _ctx.FormDetail
                       join Appfunc in _ctx.ApplicationFunctionalities on f1.Id equals Appfunc.FormId
                       join PTD in _ctx.PermissionTemplateDetail on Appfunc.Id equals PTD.FunctionalityId
                       where Appfunc.IsMenuItem == true
                       group f1 by f1.DisplayName into g
                       orderby g.FirstOrDefault().DisplayOrder ascending
                       select new Items
                       {
                           ItemName = g.FirstOrDefault().DisplayName,
                           Icon = g.FirstOrDefault().IconCode,
                           SubMenuItems = (
                           from Af in _ctx.ApplicationFunctionalities
                           join f2 in _ctx.FormDetail on Af.FormId equals f2.Id
                           join f5 in _ctx.PermissionTemplateDetail on Af.Id equals f5.FunctionalityId
                           where Af.IsMenuItem == true && Af.MenuReferenceName == g.FirstOrDefault().DisplayName && f5.TemplateId == RoleId
                           select new SubMenuItems
                           {
                               submenuItem = Af.FunctionalityName,
                               NavigationLink = "/" + f2.ControllerName + "/" + Af.ActionMethodName
                           }
                           ).ToList()
                       }
                       ).ToListAsync();

        return items;
    }
}
