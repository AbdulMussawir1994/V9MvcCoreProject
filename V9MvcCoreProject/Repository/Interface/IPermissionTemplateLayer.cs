using V9MvcCoreProject.Entities.DTOs;
using V9MvcCoreProject.Entities.ViewModels;
using V9MvcCoreProject.Helpers;

namespace V9MvcCoreProject.Repository.Interface;

public interface IPermissionTemplateLayer
{
    List<PermissionTemplateDetails> GetAllFunctionalitiesAsync();
    Task<WebResponse<ActionResponseDto>> SavePermissionTemplateAsync(PermissionTemplateViewModel model);
    Task<bool> TemplateNameExistsAsync(string TemplateName);
}
