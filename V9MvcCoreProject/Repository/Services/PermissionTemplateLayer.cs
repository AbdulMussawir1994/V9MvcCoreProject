using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> TemplateNameExistsAsync(string TemplateName)
        {
            return await _context.PermissionTemplate.AnyAsync(x => x.TemplateName == TemplateName);
        }

        public async Task<WebResponse<ActionResponseDto>> SavePermissionTemplateAsync(PermissionTemplateViewModel model)
        {
            var existUser = await _context.PermissionTemplate.Where(x => x.TemplateName == model.TemplateName).FirstOrDefaultAsync();

            if (existUser is not null)
            {
                return WebResponse<ActionResponseDto>.Failed("Template Name already exists. Please use a different name.");
            }

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
        public async Task<WebResponse<List<PermissionTemplateDto>>> GetPermissionTemplatesAsync()
        {
            var templates = await _context.PermissionTemplate
                .AsNoTracking()  // ✅ better performance (read-only query)
                .Where(pt => pt.IsActive)
                .OrderBy(pt => pt.TemplateName)
                .Select(pt => new PermissionTemplateDto
                {
                    Id = pt.Id,
                    TemplateName = pt.TemplateName
                })
                .ToListAsync();

            return WebResponse<List<PermissionTemplateDto>>.Success(templates);
        }

        public async Task<WebResponse<List<PermissionTemplateDto>>> GetAllPermissionTemplatesAsync()
        {
            var templates = await _context.PermissionTemplate
                .AsNoTracking()
                .Where(x => x.IsActive) // ✅ Optional: Filter only active ones if applicable
                .OrderBy(x => x.TemplateName) // ✅ Improves UX consistency
                .Select(x => new PermissionTemplateDto
                {
                    Id = x.Id,
                    TemplateName = x.TemplateName
                })
                .ToListAsync();

            if (!templates.Any())
            {
                return WebResponse<List<PermissionTemplateDto>>.UnSuccess(new List<PermissionTemplateDto>(), "No permission templates found.");
            }

            return WebResponse<List<PermissionTemplateDto>>.Success(
                templates,
                "Permission templates retrieved successfully.",
                true
            );
        }

        public async Task<WebResponse<PermissionTemplateViewModel>> GetPermissionTemplateById(int tempId)
        {
            var viewModel = new PermissionTemplateViewModel();

            var templateDetails = await _context.PermissionTemplate.Where(x => x.Id == tempId).FirstOrDefaultAsync();

            if (templateDetails is not null)
            {
                var activePermission = await _context.PermissionTemplateDetail
                    .Where(x => x.TemplateId == tempId)
                    .Select(x => x.FunctionalityId)
                    .ToListAsync();

                var response = (from fun in _context.ApplicationFunctionalities
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

                foreach (var item in response.Where(t => activePermission.Contains(t.FunctionalityId)))
                {
                    item.IsAllow = true;
                }
                viewModel.Id = templateDetails.Id;
                viewModel.TemplateName = templateDetails.TemplateName;
                viewModel.IsActive = templateDetails.IsActive;
                viewModel.permissionTemplates = response;
                return WebResponse<PermissionTemplateViewModel>.Success(viewModel, "Success");

            }
            else
            {
                return WebResponse<PermissionTemplateViewModel>.UnSuccess(new PermissionTemplateViewModel(), "Permission Tempalte Not Found");
            }

        }

        public async Task<WebResponse<ActionResponseDto>> UpdatePermissionTemplateAsync(PermissionTemplateViewModel model)
        {

            if (!model.IsActive)
            {
                string response = CheckUsersExistingRoles(model.Id);
                if (!string.IsNullOrEmpty(response))
                {
                    return WebResponse<ActionResponseDto>.Failed(response);
                }
            }


            var dbContextTransaction = _context.Database.BeginTransaction();
            try
            {
                List<PermissionTemplateDetail> templateDetails = new List<PermissionTemplateDetail>();
                var updatePermissionTemplate = await _context.PermissionTemplate.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                if (updatePermissionTemplate is not null)
                {
                    updatePermissionTemplate.TemplateName = model.TemplateName;
                    updatePermissionTemplate.IsActive = model.IsActive;
                    updatePermissionTemplate.UpdatedBy = model.UpdatedBy;
                    updatePermissionTemplate.UpdatedDate = DateTime.Now;
                    await _context.SaveChangesAsync();

                    _context.PermissionTemplateDetail.RemoveRange(_context.PermissionTemplateDetail.Where(x => x.TemplateId == model.Id));
                    await _context.SaveChangesAsync();

                    foreach (var item in model.permissionTemplates)
                    {
                        PermissionTemplateDetail templateDetail = new PermissionTemplateDetail
                        {
                            TemplateId = model.Id,
                            FormName = item.FormName,
                            FunctionalityId = item.FunctionalityId,
                            IsAllow = item.IsAllow
                        };
                        templateDetails.Add(templateDetail);
                    }

                    _context.PermissionTemplateDetail.AddRange(templateDetails);
                    await _context.SaveChangesAsync();
                    await dbContextTransaction.CommitAsync();
                    return WebResponse<ActionResponseDto>.Success(new ActionResponseDto()
                    {
                        Success = true,
                        ErrorMessage = string.Empty
                    });
                }
                else
                {
                    return WebResponse<ActionResponseDto>.Failed("No Record Found.");
                }

            }
            catch (Exception ex)
            {
                return WebResponse<ActionResponseDto>.Failed(ex.InnerException?.Message ?? ex.Message);
            }

        }

        public async Task<WebResponse<ActionResponseDto>> UpdatePermissionTemplateAsync1(PermissionTemplateViewModel newModel, PermissionTemplateViewModel oldModel)
        {
            if (!newModel.IsActive)
            {
                string response = CheckUsersExistingRoles(newModel.Id);
                if (!string.IsNullOrEmpty(response))
                {
                    return WebResponse<ActionResponseDto>.Failed(response);
                }
            }

            using var dbContextTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // ✅ 1. Update main PermissionTemplate
                var updatePermissionTemplate = await _context.PermissionTemplate
                    .FirstOrDefaultAsync(x => x.Id == newModel.Id);

                if (updatePermissionTemplate == null)
                    return WebResponse<ActionResponseDto>.Failed("No record found.");

                updatePermissionTemplate.TemplateName = newModel.TemplateName;
                updatePermissionTemplate.IsActive = newModel.IsActive;
                updatePermissionTemplate.UpdatedBy = newModel.UpdatedBy;
                updatePermissionTemplate.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();

                // ✅ 2. Prepare easy lookup lists for comparison
                var newAllowed = newModel.permissionTemplates
                    .Where(x => x.IsAllow)
                    .Select(x => x.FunctionalityId)
                    .ToHashSet();

                var oldAllowed = oldModel.permissionTemplates
                    .Where(x => x.IsAllow)
                    .Select(x => x.FunctionalityId)
                    .ToHashSet();

                // ✅ 3. Find new permissions to ADD (in new, not in old)
                var toAdd = newAllowed.Except(oldAllowed).ToList();

                // ✅ 4. Find permissions to REMOVE (in old, not in new)
                var toRemove = oldAllowed.Except(newAllowed).ToList();

                // ✅ 5. ADD new permissions
                foreach (var funcId in toAdd)
                {
                    var newPermission = new PermissionTemplateDetail
                    {
                        TemplateId = newModel.Id,
                        FunctionalityId = funcId,
                        FormName = newModel.permissionTemplates
                            .First(x => x.FunctionalityId == funcId).FormName,
                        IsAllow = true,
                    };
                    await _context.PermissionTemplateDetail.AddAsync(newPermission);
                }

                // ✅ 6. REMOVE old permissions
                var removeList = await _context.PermissionTemplateDetail
                    .Where(x => x.TemplateId == newModel.Id && toRemove.Contains(x.FunctionalityId))
                    .ToListAsync();

                _context.PermissionTemplateDetail.RemoveRange(removeList);

                // ✅ 7. Save all changes in one transaction
                await _context.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();

                return WebResponse<ActionResponseDto>.Success(new ActionResponseDto
                {
                    Success = true,
                    ErrorMessage = string.Empty
                });
            }
            catch (Exception ex)
            {
                await dbContextTransaction.RollbackAsync();
                return WebResponse<ActionResponseDto>.Failed(ex.InnerException?.Message ?? ex.Message);
            }
        }

        #region Private Methods


        private string CheckUsersExistingRoles(int roleId)
        {
            var users = _context.Users
                           .Where(u => u.RoleTemplateId == roleId)
                           .Select(u => u.UserName)
                           .ToList();

            if (users.Any())
            {
                return $"Role cannot be inactive. Assigned Users: {string.Join(", ", users)}.";
            }

            return string.Empty;
        }

        #endregion
    }
}
