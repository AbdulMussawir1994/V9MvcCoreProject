using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using V9MvcCoreProject.Entities.ViewModels;
using V9MvcCoreProject.Extensions.LogsHelpers.Interface;
using V9MvcCoreProject.Helpers;
using V9MvcCoreProject.Middleware.PermissionAttribute;
using V9MvcCoreProject.Repository.Interface;

namespace V9MvcCoreProject.Controllers;

public class PermissionController : BaseController
{
    private readonly IPermissionTemplateLayer _permission;

    public PermissionController(IActivityHistory ActivityLog, IPermissionTemplateLayer permission) : base(ActivityLog)
    {
        _permission = permission;
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
}