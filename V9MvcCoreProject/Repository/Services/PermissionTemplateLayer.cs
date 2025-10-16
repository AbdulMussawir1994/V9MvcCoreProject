using V9MvcCoreProject.DataDbContext;
using V9MvcCoreProject.Entities.DTOs;
using V9MvcCoreProject.Entities.Models;
using V9MvcCoreProject.Entities.ViewModels;
using V9MvcCoreProject.Extensions.LogsHelpers.Interface;
using V9MvcCoreProject.Helpers;
using V9MvcCoreProject.Repository.Interface;

namespace V9MvcCoreProject.Repository.Services
{
    public class PermissionTemplateLayer : IPermissionTemplateLayer
    {
        private readonly ApplicationDbContext _context;

        public PermissionTemplateLayer(ApplicationDbContext context, IActivityHistory ActivityLog)
        {
            _context = context;
        }

        public List<PermissionTemplateDetails> GetAllFunctionalitiesAsync()
        {
            var response = new List<PermissionTemplateDetails>();

            response = (from fun in _context.ApplicationFunctionalities
                        join form in _context.FormDetail on fun.FormId equals form.Id
                        where form.IsActive == true && fun.IsActive == true
                        select new PermissionTemplateDetails
                        {
                            FormId = form.Id,
                            FormName = form.FormName,
                            FormDisplayName = form.DisplayName,
                            FunctionalityId = fun.Id,
                            FunctionalityName = fun.FunctionalityName,
                            IsAllow = false
                        }).ToList();

            return response;
        }

        public async Task<WebResponse<ActionResponseDto>> SavePermissionTemplateAsync(PermissionTemplateViewModel model)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var permissionTemplate = new PermissionTemplate
                {
                    TemplateName = model.TemplateName,
                    IsActive = model.IsActive,
                    CreatedBy = model.CreatedBy,
                    CreatedDate = DateTime.UtcNow
                };

                await _context.PermissionTemplate.AddAsync(permissionTemplate);
                await _context.SaveChangesAsync();

                // Bulk prepare details
                if (model.permissionTemplates?.Count > 0)
                {
                    var details = model.permissionTemplates.Select(item => new PermissionTemplateDetail
                    {
                        TemplateId = permissionTemplate.Id,
                        FormName = item.FormName,
                        FunctionalityId = item.FunctionalityId,
                        IsAllow = item.IsAllow
                    }).ToList();

                    await _context.PermissionTemplateDetail.AddRangeAsync(details);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return WebResponse<ActionResponseDto>.Success(new ActionResponseDto
                {
                    Success = true,
                    ErrorMessage = string.Empty,
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return WebResponse<ActionResponseDto>.Failed(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
