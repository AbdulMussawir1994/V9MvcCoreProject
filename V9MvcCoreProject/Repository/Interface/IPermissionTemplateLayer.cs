using V9MvcCoreProject.Entities.DTOs;
using V9MvcCoreProject.Entities.ViewModels;
using V9MvcCoreProject.Helpers;

namespace V9MvcCoreProject.Repository.Interface;

public interface IPermissionTemplateLayer
{
    List<PermissionTemplateDetails> GetAllFunctionalitiesAsync();
    Task<WebResponse<ActionResponseDto>> SavePermissionTemplateAsync(PermissionTemplateViewModel model);
    Task<bool> TemplateNameExistsAsync(string TemplateName);
    Task<WebResponse<List<PermissionTemplateDto>>> GetPermissionTemplatesAsync();
    Task<WebResponse<List<PermissionTemplateDto>>> GetAllPermissionTemplatesAsync();
    Task<WebResponse<PermissionTemplateViewModel>> GetPermissionTemplateById(int tempId);
    Task<WebResponse<ActionResponseDto>> UpdatePermissionTemplateAsync(PermissionTemplateViewModel model);

    Task<WebResponse<ActionResponseDto>> UpdatePermissionTemplateAsync1(PermissionTemplateViewModel newModel, PermissionTemplateViewModel oldModel);
}
