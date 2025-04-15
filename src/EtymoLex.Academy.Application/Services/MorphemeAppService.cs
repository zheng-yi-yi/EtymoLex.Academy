using System;
using System.Linq;
using System.Threading.Tasks;
using EtymoLex.Academy.Permissions;
using Microsoft.EntityFrameworkCore;
using EtymoLex.Academy.Dtos.Morpheme;
using EtymoLex.Academy.Dtos;
using System.Collections.Generic;
using EtymoLex.Academy.Enums;
using EtymoLex.Academy.Helper;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Validation;
using Volo.Abp;
using Volo.Abp.Uow;

namespace EtymoLex.Academy;


public class MorphemeAppService : NameObjectAppService<Morpheme, MorphemeDto, Guid, MorphemeGetListInput, CreateUpdateMorphemeDto, ExportMorphemeDto>,
    IMorphemeAppService
{
    protected override string GetPolicyName { get; set; } = AcademyPermissions.Morpheme.Default;
    protected override string GetListPolicyName { get; set; } = AcademyPermissions.Morpheme.Default;
    protected override string CreatePolicyName { get; set; } = AcademyPermissions.Morpheme.Create;
    protected override string UpdatePolicyName { get; set; } = AcademyPermissions.Morpheme.Edit;
    protected override string DeletePolicyName { get; set; } = AcademyPermissions.Morpheme.Delete;
    protected override string CopyPolicyName { get; set; } = AcademyPermissions.Morpheme.Copy;
    protected override string ExportPolicyName { get; set; } = AcademyPermissions.Morpheme.Export;
    protected override string ImportPolicyName { get; set; } = AcademyPermissions.Morpheme.Import;

    private readonly IMorphemeRepository _repository;

    public MorphemeAppService(IMorphemeRepository repository) : base(repository)
    {
        _repository = repository;

    }

    protected override async Task<IQueryable<Morpheme>> CreateFilteredQueryAsync(MorphemeGetListInput input)
    {
        // TODO: AbpHelper generated
        return (await base.CreateFilteredQueryAsync(input))
            .WhereIf(input.Filter != null, x => EF.Functions.Like(x.Name.ToLower(), '%' + (input.Filter ?? "").ToLower() + '%'))
            .WhereIf(!input.Value.IsNullOrWhiteSpace(), x => x.Value.Contains(input.Value))
            .WhereIf(input.Type != null, x => x.Type == input.Type)
            .WhereIf(!input.OriginLanguage.IsNullOrWhiteSpace(), x => x.OriginLanguage.Contains(input.OriginLanguage))
            .WhereIf(!input.Meaning.IsNullOrWhiteSpace(), x => x.Meaning.Contains(input.Meaning))
            ;
    }


    [UnitOfWork]
    public override async Task<ImportResultDto> Import(IEnumerable<ExportMorphemeDto> dtos, OverridingMode mode)
    {
        ImportResultDto result = new ImportResultDto();
        result.Status = true;

        foreach (var dto in dtos)
        {
            try
            {
                var entity = await (await Repository.GetQueryableAsync())
                .Where(x => x.Value == dto.Value)
                .FirstOrDefaultAsync();

                if (entity == null)
                {
                    Morpheme entityNew = ObjectMapper.Map<ExportMorphemeDto, Morpheme>(dto);
                    entityNew.Name = dto.Value;
                    entityNew.DisplayName = dto.Value;
                    ValidateEntity(dto);
                    CommonHelper.SetPropertyValue(entityNew, "TenantId", CurrentTenant.Id);
                    CommonHelper.SetPropertyValue(entityNew, "CreationTime", DateTime.Now);
                    await Repository.InsertAsync(entityNew);
                }
                else
                {
                    if (mode == OverridingMode.Overwrite)
                    {
                        AcademyApplicationAutoMapperProfile.MapNotNullProperty(dto, entity);
                        entity.Name = dto.Value;
                        entity.DisplayName = dto.Value;
                        await Repository.UpdateAsync(entity);
                    }
                    else if (mode == OverridingMode.Skip)
                    {
                        continue;
                    }
                    else
                    {
                        string errorDetail = ErrorDetailHelper.GetErrorDetail(UFEConsts.AlreadyExists, UFEConsts.ModelingExisted);
                        throw new UserFriendlyException(L["ERROR_ModelingExisted"], AcademyDomainErrorCodes.ModelingAlreadyExisted, errorDetail);
                    }
                }
            }
            catch (UserFriendlyException ex) when (ex.Code == AcademyDomainErrorCodes.ModelingAlreadyExisted)
            {
                string errorDetail = ErrorDetailHelper.GetErrorDetail(UFEConsts.AlreadyExists, UFEConsts.ModelingExisted);
                throw new UserFriendlyException(L["ERROR_ModelingExisted"], AcademyDomainErrorCodes.ModelingAlreadyExisted, errorDetail);
            }
            catch (AbpValidationException ex)
            {
                AddOneErrorItem(result, dto.Value, ex.ValidationErrors.FirstOrDefault()?.ErrorMessage);
            }
            catch (Exception ex)
            {
                AddOneErrorItem(result, dto.Value, ex.Message);
            }
        }

        if (result.Items.Count > 0)
        {
            result.Status = false;
        }
        return result;
    }
}
