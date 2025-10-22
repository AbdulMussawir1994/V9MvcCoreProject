using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using V9MvcCoreProject.Entities.DTOs;
using V9MvcCoreProject.Entities.ViewModels;
using V9MvcCoreProject.Extensions.LogsHelpers.Interface;
using V9MvcCoreProject.Helpers;
using V9MvcCoreProject.Middleware.PermissionAttribute;
using V9MvcCoreProject.Repository.Interface;

namespace V9MvcCoreProject.Controllers;

public class PermissionController : BaseController
{
    private readonly IPermissionTemplateLayer _permission;
    private readonly IUserAccessServiceLayer _userAccessService;

    public PermissionController(IActivityHistory ActivityLog, IPermissionTemplateLayer permission, IUserAccessServiceLayer userAccessService) : base(ActivityLog)
    {
        _permission = permission;
        _userAccessService = userAccessService;
    }

    [ServiceFilter(typeof(CheckUserPermission))]
    public IActionResult PermissionTemplate()
    {
        var model = new PermissionTemplateViewModel();
        var result = _permission.GetAllFunctionalitiesAsync();

        model.permissionTemplates = result;
        return View(model);
    }

    //[CheckUserSession]
    [HttpPost]
    public async Task<JsonResult> SavePermissionTemplate(PermissionTemplateViewModel model)
    {
        //if (!ModelState.IsValid)
        //    return Json(new { success = false, message = "Invalid data." });

        model.CreatedBy = this.GetLoggedInUserId();
        model.IsActive = true;
        model.CreatedDate = DateTime.UtcNow;

        var result = await _permission.SavePermissionTemplateAsync(model);
        await this.InsertActivityInsert(string.Empty, model, "ADD");

        if (!result.IsSuccess)
        {
            return Json(new { success = result.IsSuccess, message = result.Message ?? "Failed due to Exception." });
        }

        return Json(new { success = result.IsSuccess, message = result.Message ?? "Saved successfully." });
    }

    public JsonResult ValidateTemplateName(string TemplateName)
    {
        bool IsExists = Task.Run(async () => await _permission.TemplateNameExistsAsync(TemplateName)).Result;
        return Json(!IsExists ? "true" : string.Format("{0} already exists.", TemplateName));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<JsonResult> GetPermissionTemplates()
    {
        var response = await _permission.GetPermissionTemplatesAsync();
        return Json(response.Value);
    }

    [HttpGet]
    [ServiceFilter(typeof(CheckUserPermission))]
    public async Task<IActionResult> ChangePermissionTemplate(int tempId)
    {
        if (tempId is not 0)
        {
            var result = await _permission.GetPermissionTemplateById(tempId);
            await this.InsertActivityInsert(string.Empty, result.Value, "VIEW");

            var response = await _permission.GetPermissionTemplatesAsync();
            var templates = new SelectList(response.Value, "Id", "TemplateName", tempId);
            ViewBag.PermissionTemplates = templates;

            if (result.IsSuccess)
            {
                return View("ChangePermissionTemplate", result.Value);
            }
            else
            {
                return View();
            }
        }
        else
        {
            var response = await _permission.GetPermissionTemplatesAsync();
            var templates = new SelectList(response.Value, "Id", "TemplateName");

            ViewBag.PermissionTemplates = templates;
            return View();
        }
    }

    [ServiceFilter(typeof(CheckUserPermission))]
    public async Task<JsonResult> UpdatePermissionTemplate(PermissionTemplateViewModel model)
    {
        ActionResponseDto response = new ActionResponseDto()
        {
            ErrorMessage = String.Empty,
            Success = true
        };

        try
        {
            model.CreatedBy = this.GetLoggedInUserId();
            int RoleId = Convert.ToInt32(HttpContext?.User?.Claims?.Where(x => x.Type == "RoleId")?.SingleOrDefault()?.Value);

            var permission = await _userAccessService.GetUserPermissionsByRoleIdAsync(RoleId);
            var serializedPermissions = JsonConvert.SerializeObject(permission);
            HttpContext?.Session?.SetString("UserPermissions", serializedPermissions);
            await this.InsertActivityInsert(await _permission.GetPermissionTemplateById(model.Id), model, "UPDATE");
            var serviceResponse = await _permission.UpdatePermissionTemplateAsync(model);
            return new JsonResult(new { success = serviceResponse.IsSuccess, serviceResponse.Message, serviceResponse.Value });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, Message = ex.Message, Data = "" });
        }
    }

}