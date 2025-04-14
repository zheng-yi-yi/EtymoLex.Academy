using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EtymoLex.Academy.EntityFrameworkCore;
using EtymoLex.Academy.Helper;
using EtymoLex.Academy.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EtymoLex.Academy.Repositories;

public abstract class NameObjectRepository<TEntity, Tkey> : NameObjectNonTenantRepository<TEntity, Tkey> where TEntity : NameObject<Tkey>
{
    protected NameObjectRepository(IDbContextProvider<AcademyDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }


}

public abstract class NameObjectNonTenantRepository<TEntity, Tkey> : EfCoreRepository<AcademyDbContext, TEntity, Tkey>
    where TEntity : NameObjectNonTenant<Tkey>
{
    public virtual bool ValidateNormalizedName { get; set; } = true;
    protected NameObjectNonTenantRepository(IDbContextProvider<AcademyDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }
    protected IStringLocalizer<AcademyResource> Localizer => LazyServiceProvider.LazyGetRequiredService<IStringLocalizer<AcademyResource>>();
    public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        entity.NormalizedName = entity.Name.ToUpperInvariant();
        entity.DisplayName = string.IsNullOrEmpty(entity.DisplayName) ? entity.Name : entity.DisplayName;

        if (ValidateNormalizedName)
        {
            var existsItem = (await GetQueryableAsync())
           .Where(m => m.NormalizedName == entity.NormalizedName).ToList()
           .FirstOrDefault(m => m.Id!.ToString() != entity.Id!.ToString());
            ;
            if (existsItem != null)
            {
                string errorDetail = ErrorDetailHelper.GetErrorDetail(UFEConsts.AlreadyExists, UFEConsts.NameExisted);
                throw new UserFriendlyException(Localizer["ERROR_NameExisted", entity.Name], AcademyDomainErrorCodes.ModelingAlreadyExisted, errorDetail);
            }
        }

        var existsDisplayNameItem = (await GetQueryableAsync())
                .Where(m => m.DisplayName == entity.DisplayName)
                .ToList()
                .FirstOrDefault(m => m.Id!.ToString() != entity.Id!.ToString());

        if (existsDisplayNameItem != null)
        {
            string errorDetail = ErrorDetailHelper.GetErrorDetail(UFEConsts.AlreadyExists, UFEConsts.DisplayNameExisted);
            throw new UserFriendlyException(Localizer["ERROR_DisplayNameExisted", entity.DisplayName], AcademyDomainErrorCodes.DisplayNameAlreadyExisted, errorDetail);
        }

        return await base.UpdateAsync(entity, autoSave, cancellationToken);
    }

    public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        entity.NormalizedName = entity.Name.ToUpperInvariant();
        entity.DisplayName = string.IsNullOrEmpty(entity.DisplayName) ? entity.Name : entity.DisplayName;

        if (ValidateNormalizedName)
            if (await FindByNormalizedNameAsync(entity.NormalizedName) != null)
            {
                string errorDetail = ErrorDetailHelper.GetErrorDetail(UFEConsts.AlreadyExists, UFEConsts.NameExisted);
                throw new UserFriendlyException(Localizer["ERROR_NameExisted", entity.Name], AcademyDomainErrorCodes.ModelingAlreadyExisted, errorDetail);
            }

        if (await FindByDisplayNameAsync(entity.DisplayName) != null)
        {
            string errorDetail = ErrorDetailHelper.GetErrorDetail(UFEConsts.AlreadyExists, UFEConsts.DisplayNameExisted);
            throw new UserFriendlyException(Localizer["ERROR_DisplayNameExisted", entity.DisplayName], AcademyDomainErrorCodes.DisplayNameAlreadyExisted, errorDetail);
        }

        return await base.InsertAsync(entity, autoSave, cancellationToken);
    }

    public override async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var updateEntities = entities.ToList();
        foreach (var entity in updateEntities)
        {
            entity.NormalizedName = entity.Name.ToUpperInvariant();
            entity.DisplayName = string.IsNullOrEmpty(entity.DisplayName) ? entity.Name : entity.DisplayName;
        }
        if (ValidateNormalizedName)
        {
            var existsList = (await GetQueryableAsync())
           .Where(m => updateEntities.Select(x => x.NormalizedName).Contains(m.NormalizedName))
           .ToList()
           ;
            var existsItem = existsList.Where(x =>
                updateEntities.Any(x => x.Id.ToString() != x.Id.ToString() && x.NormalizedName == x.NormalizedName));
            if (existsItem.Count() > 0)
            {
                string errorDetail = ErrorDetailHelper.GetErrorDetail(UFEConsts.AlreadyExists, UFEConsts.NameExisted);
                throw new UserFriendlyException(Localizer["ERROR_NameExisted", existsItem.First().Name], AcademyDomainErrorCodes.ModelingAlreadyExisted, errorDetail);
            }
        }

        var existsDisplayNameList = (await GetQueryableAsync())
                .Where(m => updateEntities.Select(x => x.DisplayName).Contains(m.DisplayName))
                .ToList();

        var existsDisplayNameItem = existsDisplayNameList.Where(x =>
            updateEntities.Any(e => e.Id.ToString() != x.Id.ToString() && e.DisplayName == x.DisplayName));

        if (existsDisplayNameItem.Any())
        {
            string errorDetail = ErrorDetailHelper.GetErrorDetail(UFEConsts.AlreadyExists, UFEConsts.DisplayNameExisted);
            throw new UserFriendlyException(Localizer["ERROR_DisplayNameExisted", existsDisplayNameItem.First().DisplayName], AcademyDomainErrorCodes.DisplayNameAlreadyExisted, errorDetail);
        }

        await base.UpdateManyAsync(updateEntities, autoSave, cancellationToken);
    }

    public override async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var updateEntities = entities.ToList();
        foreach (var entity in updateEntities)
        {
            entity.NormalizedName = entity.Name.ToUpperInvariant();
            entity.DisplayName = string.IsNullOrEmpty(entity.DisplayName) ? entity.Name : entity.DisplayName;
        }
        if (ValidateNormalizedName)
        {
            var existsList = (await GetQueryableAsync())
            .Where(m => updateEntities.Any(x => x.NormalizedName == m.NormalizedName));
            if (existsList.Count() > 0)
            {
                string errorDetail = ErrorDetailHelper.GetErrorDetail(UFEConsts.AlreadyExists, UFEConsts.NameExisted);
                throw new UserFriendlyException(Localizer["ERROR_NameExisted", existsList.First().Name], AcademyDomainErrorCodes.ModelingAlreadyExisted, errorDetail);
            }
        }

        var existsDisplayNameList = (await GetQueryableAsync())
                .Where(m => updateEntities.Any(x => x.DisplayName == m.DisplayName));

        if (existsDisplayNameList.Any())
        {
            string errorDetail = ErrorDetailHelper.GetErrorDetail(UFEConsts.AlreadyExists, UFEConsts.DisplayNameExisted);
            throw new UserFriendlyException(Localizer["ERROR_DisplayNameExisted", existsDisplayNameList.First().DisplayName], AcademyDomainErrorCodes.DisplayNameAlreadyExisted, errorDetail);
        }
        await base.InsertManyAsync(updateEntities, autoSave, cancellationToken);
    }

    public virtual async Task<TEntity> FindByNormalizedNameAsync(string normalizedName)
    {
        return await (await GetQueryableAsync()).FirstOrDefaultAsync(m => m.NormalizedName == normalizedName);
    }
    public virtual async Task<TEntity> FindByDisplayNameAsync(string displayName)
    {
        return await (await GetQueryableAsync()).FirstOrDefaultAsync(m => m.DisplayName == displayName);
    }
}