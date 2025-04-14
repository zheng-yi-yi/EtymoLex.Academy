using EtymoLex.Academy;
using EtymoLex.Academy.Dtos;
using EtymoLex.Academy.Localization;
using EtymoLex.Academy.Attributes;
using EtymoLex.Academy.Enums;
using EtymoLex.Academy.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using Volo.Abp.EntityFrameworkCore;


namespace Molex.UFE.Services
{
    public abstract class NameObjectAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateUpdateInput, TExportInput> : CrudAppService<
            TEntity, //The Modeling entity
            TEntityDto, //Used to show Modeling
            TKey, //Primary key of the Modeling entity
            TGetListInput, //Used for paging/sorting
            TCreateUpdateInput,
            TCreateUpdateInput>
        where TEntity : NameObject<TKey>
        where TEntityDto : NameObjectDto<TKey>, new()
        where TCreateUpdateInput : CreateUpdateNameObjectDto
        where TExportInput : class
    {

        protected virtual string CopyPolicyName { get; set; } = string.Empty;
        protected virtual string ImportPolicyName { get; set; } = string.Empty;
        protected virtual string ExportPolicyName { get; set; } = string.Empty;
        protected virtual IAuditingManager AuditingManager => LazyServiceProvider.GetRequiredService<IAuditingManager>();

        private const string NORMALIZED_NAME = "NormalizedName";

        protected NameObjectAppService(IRepository<TEntity, TKey> repository) : base(repository)
        {
            LocalizationResource = typeof(AcademyResource);
        }

        public virtual async Task<TEntityDto> CreateOrUpdateAsync(TCreateUpdateInput data)
        {
            data.Name = data.Name.Trim();
            var dbContext = await Repository.GetDbContextAsync();
            var dbSet = dbContext.Set<TEntity>();
            var trimmedUpperName = data.Name.ToUpper();
            var query = from e in dbContext.Set<TEntity>()
                        where e.NormalizedName == trimmedUpperName
                        select e;
            if (query.Any())
            {
                var id = query.First().Id;
                await CheckPolicyAsync(UpdatePolicyName).ConfigureAwait(continueOnCapturedContext: false);
                return await this.UpdateAsync(id, data);

            }
            else
            {
                await CheckPolicyAsync(CreatePolicyName).ConfigureAwait(continueOnCapturedContext: false);
                return await this.CreateAsync(data);
            }
        }

        public override async Task<TEntityDto> CreateAsync(TCreateUpdateInput input)
        {
            input.Name = input.Name.Trim();
            return await base.CreateAsync(input);
        }

        public override async Task<TEntityDto> UpdateAsync(TKey id, TCreateUpdateInput input)
        {
            input.Name = input.Name.Trim();
            return await base.UpdateAsync(id, input);
        }

        public virtual async Task<IEnumerable<TEntityDto>> GetAllInstancesAsync()
        {
            await CheckPolicyAsync(GetPolicyName).ConfigureAwait(continueOnCapturedContext: false);
            var query = await Repository.GetQueryableAsync();
            var s = query.ToQueryString();
            var list = query.Select(x => new TEntityDto()
            {
                Name = x.Name,
                Id = x.Id,
                DisplayName = x.DisplayName
            }).OrderBy(x => x.Name).ToList();
            return list;
        }

        public virtual async Task<TEntityDto?> GetByNameAsync(string name)
        {
            await CheckPolicyAsync(GetPolicyName).ConfigureAwait(continueOnCapturedContext: false);
            var dbContext = await Repository.GetDbContextAsync();
            var dbSet = dbContext.Set<TEntity>();
            var query = from e in dbContext.Set<TEntity>()
                        where e.Name == name
                        select ObjectMapper.Map<TEntity, TEntityDto>(e);
            return query.FirstOrDefault();
        }
        [UnitOfWork]
        [HttpPut]
        public virtual async Task<IEnumerable<TEntityDto>> MultipleUpdateAsync(Dictionary<TKey, TCreateUpdateInput> inputs)
        {
            await CheckPolicyAsync(UpdatePolicyName).ConfigureAwait(continueOnCapturedContext: false);
            var list = new List<TEntityDto>();
            foreach (var key in inputs.Keys)
            {
                list.Add(await base.UpdateAsync(key, inputs[key]));
            }
            return list;

        }

        [UnitOfWork]
        public virtual async Task<ImportResultDto> Import(IEnumerable<TExportInput> dtos, OverridingMode mode)
        {
            await CheckPolicyAsync(ImportPolicyName).ConfigureAwait(continueOnCapturedContext: false);

            ImportResultDto result = new ImportResultDto();
            result.Status = true;

            foreach (var dto in dtos)
            {
                string name = CommonHelper.GetPropertyValueByName(dto, UFEConsts.ModelSearchField)?.ToString()?.Trim();
                string? displayName = CommonHelper.GetPropertyValueByName(dto, UFEConsts.DisplayNameField)?.ToString()?.Trim();

                if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(displayName))
                {
                    continue;
                }

                try
                {
                    TEntity? entity = await GetEntityByNameOrDisplayNameAsync(name, displayName, dto, Repository);

                    if (entity == null)
                    {
                        TEntity entityNew = ObjectMapper.Map<TExportInput, TEntity>(dto);

                        ValidateEntity(dto);
                        SetDisplayNameValue(dto, name, entityNew);
                        CommonHelper.SetPropertyValue(entityNew, "TenantId", CurrentTenant.Id);
                        CommonHelper.SetPropertyValue(entityNew, "CreationTime", DateTime.Now);
                        await SetImportDtoCustomAttributeAync(dto, entityNew);
                        entityNew.Name = name;
                        await Repository.InsertAsync(entityNew);
                    }
                    else
                    {
                        if (mode == OverridingMode.Overwrite)
                        {
                            AcademyApplicationAutoMapperProfile.MapNotNullProperty(dto, entity);
                            await SetImportDtoCustomAttributeAync(dto, entity);
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
                    AddOneErrorItem(result, name, ex.ValidationErrors.FirstOrDefault()?.ErrorMessage);
                }
                catch (Exception ex)
                {
                    AddOneErrorItem(result, name, ex.Message);
                }
            }

            if (result.Items.Count > 0)
            {
                result.Status = false;
            }
            return result;
        }

        protected async Task<TEntity?> GetEntityByNameOrDisplayNameAsync(string name, string displayName, TExportInput dto, IRepository<TEntity, TKey> repository)
        {
            TEntity? entity = await GetEntityByNameAsync(name, dto, repository);
            if (entity == null && !string.IsNullOrEmpty(displayName))
            {
                entity = await GetEntityByDisplayNameAsync(displayName, dto, repository);
            }
            return entity;
        }

        private async Task<TEntity?> GetEntityByNameAsync(string name, TExportInput dto, IRepository<TEntity, TKey> repository)
        {
            var dbContext = await repository.GetDbContextAsync();
            var dbSet = dbContext.Set<TEntity>();
            var query = dbSet.Where(GenerateQuery(UFEConsts.ModelSearchField, name));
            return await query.SingleOrDefaultAsync();
        }

        private async Task<TEntity?> GetEntityByDisplayNameAsync(string displayName, TExportInput dto, IRepository<TEntity, TKey> repository)
        {
            var dbContext = await repository.GetDbContextAsync();
            var dbSet = dbContext.Set<TEntity>();
            var query = dbSet.Where(GenerateQuery(UFEConsts.DisplayNameField, displayName));
            return await query.FirstOrDefaultAsync();
        }

        private static void SetDisplayNameValue(TExportInput dto, string name, TEntity entityNew)
        {
            var displayName = CommonHelper.GetPropertyValueByName(dto, UFEConsts.DisplayNameField);
            if (displayName == null || string.IsNullOrEmpty(displayName.ToString()))
            {
                CommonHelper.SetPropertyValue(entityNew, UFEConsts.DisplayNameField, name);
            }
            var description = CommonHelper.GetPropertyValueByName(dto, UFEConsts.DescriptionField);
            if (description == null || string.IsNullOrEmpty(description.ToString()))
            {
                CommonHelper.SetPropertyValue(entityNew, UFEConsts.DescriptionField, "");
            }
        }

        protected void AddOneErrorItem(ImportResultDto result, string name, string errorMessage)
        {
            ImportResultItemDto item = new ImportResultItemDto();
            item.ErrorMessage = errorMessage;
            item.Name = name;
            result.Items.Add(item);
        }

        private void ValidateEntity(TExportInput dto)
        {
            PropertyInfo[] propertyInfos = dto.GetType().GetProperties();

            foreach (PropertyInfo property in propertyInfos)
            {
                if (property.Name == "Description" ||
                    property.Name == "LastModificationTime" ||
                    property.Name == "CreationTime" ||
                    property.Name == "TenantName" ||
                    property.Name == "ExtraProperties" ||
                    property.GetCustomAttribute<OptionalAttribute>() != null ||
                    (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    continue;
                }

                if (property.IsDefined(typeof(NotRequiredAttribute), false))
                {
                    continue;
                }

                if (property != null && property.CanRead)
                {

                    if (property.GetValue(dto) == null || string.IsNullOrEmpty(property.GetValue(dto).ToString()))
                    {
                        string errorDetail = ErrorDetailHelper.GetErrorDetail(UFEConsts.AlreadyExists, UFEConsts.ModelingRequiredDataNull);
                        throw new UserFriendlyException(L["ERROR_ModelingRequiredDataNull", property.Name], AcademyDomainErrorCodes.ModelingRequiredDataIsNull, errorDetail);
                    }
                }
            }
        }

        [UnitOfWork]
        public virtual async Task<bool> MultipleDelete(IEnumerable<TKey> ids)
        {
            await CheckPolicyAsync(DeletePolicyName).ConfigureAwait(continueOnCapturedContext: false);

            await Repository.DeleteManyAsync(ids);
            return true;
        }

        public virtual async Task<TEntityDto> CopyAsync(TCreateUpdateInput input)
        {
            var originalPolicyName = CreatePolicyName;
            try
            {
                CreatePolicyName = new string(CopyPolicyName);
                return await CreateAsync(input);
            }
            finally
            {
                CreatePolicyName = originalPolicyName;
            }
        }

        public virtual async Task<byte[]> Export(FileType fileType, IEnumerable<TKey> ids)
        {
            await CheckPolicyAsync(ExportPolicyName).ConfigureAwait(continueOnCapturedContext: false);

            var query = await Repository.GetQueryableAsync();

            List<TEntity> data = query.Where(h => ids.Contains(h.Id)).ToList();
            List<TExportInput> dtos = data.Select(x => ObjectMapper.Map<TEntity, TExportInput>(x)).ToList();
            await SetExportDtoCustomAttributeAync(dtos, data);

            string fileName = CommonHelper.GetDownloadFileName(typeof(TEntity).Name, fileType);

            if (fileType == FileType.Excel)
            {
                using (MemoryStream ms = ExcelHelper<TExportInput>.WriteObjectToExcelStream(dtos))
                {
                    return ms.ToArray();
                }

            }
            using (MemoryStream jsonMs = CommonHelper.SerializeObjectToJsonStream(dtos))
            {
                return jsonMs.ToArray();
            }
        }

        public virtual async Task<byte[]> ExportAll(FileType fileType)
        {
            await CheckPolicyAsync(ExportPolicyName).ConfigureAwait(continueOnCapturedContext: false);
            var query = await Repository.GetQueryableAsync();
            List<TEntity> data = await query.ToListAsync();
            List<TExportInput> dtos = data.Select(x => ObjectMapper.Map<TEntity, TExportInput>(x)).ToList();
            await SetExportDtoCustomAttributeAync(dtos, data);

            if (fileType == FileType.Excel)
            {
                using (MemoryStream ms = ExcelHelper<TExportInput>.WriteObjectToExcelStream(dtos))
                {
                    return ms.ToArray();
                }

            }
            using (MemoryStream jsonMs = CommonHelper.SerializeObjectToJsonStream(dtos))
            {
                return jsonMs.ToArray();
            }
        }

        public virtual async Task<PagedResultDto<ModelingHistoryDto>> GetModelingHistory(ModelingInputDto<TKey> input)
        {
            await CheckPolicyAsync(GetPolicyName).ConfigureAwait(continueOnCapturedContext: false);

            var context = Repository.GetDbContext();

            var query = from auditLog in context.Set<AuditLog>()
                        join entityChange in context.Set<EntityChange>() on auditLog.Id equals entityChange.AuditLogId
                        //join propertyChange in context.Set<EntityPropertyChange>() on entityChange.Id equals propertyChange.EntityChangeId
                        where entityChange.EntityId == input.Id.ToString()
                        select new { entityChange.Id, auditLog.UserName, auditLog.ExecutionTime, entityChange.ChangeType };

            //Paging
            var pageingQuery = query
                .OrderBy(m => m.ExecutionTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            //Execute the query and get a list
            var queryResult = await AsyncExecuter.ToListAsync(pageingQuery);

            //Convert the query result to a list of BookDto objects
            var modelingHistoryDtos = queryResult.Select(x =>
            {
                var historyDto = new ModelingHistoryDto();
                historyDto.Id = x.Id;
                historyDto.UserName = x.UserName;
                historyDto.ExecutionTime = x.ExecutionTime.ToString();
                historyDto.ChangeType = x.ChangeType.ToString();
                return historyDto;
            }).ToList();


            foreach (var item in modelingHistoryDtos)
            {
                await SetHistoryProperty(context, item);
            }

            //Get the total count
            var totalCount = query.Count();
            return new PagedResultDto<ModelingHistoryDto>(
                totalCount,
                modelingHistoryDtos
            );
        }

        protected void AddModelingRelationIdChangeInLog(TEntityDto? orignal, TEntityDto target, string propertyName, string propertyUIName, string valueProperty)
        {
            var currentAuditLogScope = AuditingManager.Current;

            var isSiteChanged = CheckSiteIsChanged(orignal, target, propertyName);

            if (currentAuditLogScope != null && isSiteChanged)
            {
                var entityChanges = currentAuditLogScope.Log.EntityChanges.FirstOrDefault(m => m.EntityId == target.Id.ToString());
                entityChanges?.PropertyChanges.Add(new EntityPropertyChangeInfo()
                {
                    PropertyName = propertyUIName,
                    OriginalValue = orignal == null ? "" : GetPropertyName(orignal, valueProperty),
                    NewValue = CommonHelper.GetPropertyValueByName(target, valueProperty).ToString(),
                    PropertyTypeFullName = typeof(string).FullName
                });
            }
        }

        private string GetPropertyName(object ojb, string property)
        {
            var propertyObject = CommonHelper.GetPropertyValueByName(ojb, property);
            string value = propertyObject == null ? "" : propertyObject.ToString();
            return value;
        }
        private static bool CheckSiteIsChanged(TEntityDto? orignal, TEntityDto target, string propertyName)
        {
            bool isChange = false;
            if (orignal == null || target == null)
            {
                isChange = true;
            }
            else
            {
                var orignalObj = CommonHelper.GetPropertyValueByName(orignal, propertyName);
                var targetObj = CommonHelper.GetPropertyValueByName(target, propertyName);
                var orignalId = orignalObj == null ? "" : orignalObj.ToString();
                var targetId = targetObj == null ? "" : targetObj.ToString();
                if (orignalId != targetId)
                {
                    isChange = true;
                }
            }
            return isChange;
        }

        private async Task SetHistoryProperty(DbContext context, ModelingHistoryDto? item)
        {
            var propertyQuery = from propertyChange in context.Set<EntityPropertyChange>()
                                where propertyChange.EntityChangeId == item.Id
                                select new { propertyChange.PropertyName, propertyChange.OriginalValue, propertyChange.NewValue };

            var propertyQueryResult = await AsyncExecuter.ToListAsync(propertyQuery);

            var historyPropertyDtos = propertyQueryResult.Select(x =>
            {
                var historyDto = new ModelingPropertyDto();
                historyDto.PropertyName = x.PropertyName;
                historyDto.OriginalValue = x.OriginalValue;
                historyDto.NewValue = x.NewValue;
                return historyDto;
            }).ToList();

            item.Children = historyPropertyDtos;
        }

        private Expression<Func<TEntity, bool>> GenerateQuery(string name, object value)
        {
            // 使用EF.Property来动态访问属性  
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var property = typeof(TEntity).GetProperty(name);

            var namePropertyAccess = Expression.Property(parameter, property);
            var nameConstant = Expression.Constant(value, value.GetType());

            if (name == "TenantId")
            {
                var nullableValue = (Guid?)value;
                nameConstant = Expression.Constant(nullableValue, typeof(Guid?));
            }
            var equalsExpression = Expression.Equal(namePropertyAccess, nameConstant);

            var lambda = Expression.Lambda<Func<TEntity, bool>>(equalsExpression, parameter);

            return lambda;
        }

        [HttpPost]
        public virtual async Task<IEnumerable<TEntityDto>> GetExistInstancesAsync(IEnumerable<TExportInput> entities)
        {
            await CheckPolicyAsync(GetPolicyName).ConfigureAwait(continueOnCapturedContext: false);

            var normalizedNames = entities.Select(x => CommonHelper.GetPropertyValueByName(x, UFEConsts.ModelSearchField)?.ToString()?.Trim())
                                .Where(n => n != null)
                                .Select(n => n!.ToUpperInvariant()).ToList();

            if (normalizedNames.Count == 0)
            {
                return new List<TEntityDto>();
            }

            var query = (await Repository.GetQueryableAsync()).Where(e => normalizedNames.Contains(e.NormalizedName));
            var data = await AsyncExecuter.ToListAsync(query);
            return data.Select(x => ObjectMapper.Map<TEntity, TEntityDto>(x)).ToList();
        }

        #region handle export/import name in related table
        private async Task SetImportDtoCustomAttributeAync(TExportInput dto, TEntity entity)
        {
            var dtoProperties = typeof(TExportInput).GetProperties();

            foreach (var dtoProp in dtoProperties)
            {
                var exportAttr = dtoProp.GetCustomAttribute<ExportAttribute>();
                if (exportAttr != null)
                {
                    var relatedIdName = exportAttr.RelatedId;
                    var relatedDomain = exportAttr.RelatedDomain;
                    var relatedPropertyName = exportAttr.RelatedProperty;
                    string relatedPropertyValue = dtoProp.GetValue(dto).ToString();

                    var entityRelatedProperty = typeof(TEntity).GetProperty(relatedIdName);
                    var relatedId = await GetEntityIdAsync(relatedDomain, relatedPropertyName, relatedPropertyValue);
                    entityRelatedProperty.SetValue(entity, relatedId);
                }
            }
        }
        private async Task<Guid> GetEntityIdAsync(string entityName, string propertyName, string propertyValue)
        {
            // 动态获取领域实体的类型
            Type entityType = Type.GetType(entityName);

            if (entityType == null)
            {
                throw new ArgumentException($"Can't find the type '{entityName}'");
            }

            // 构造 IRepository<TEntity, Guid> 类型
            Type repositoryType = typeof(IRepository<,>).MakeGenericType(entityType, typeof(Guid));

            // 获取 IRepository 实例
            var repository = LazyServiceProvider.GetRequiredService(repositoryType);

            if (repository == null)
            {
                throw new InvalidOperationException($"can't find repository: '{repositoryType.FullName}'");
            }

            // 使用反射调用 GetAsync 方法GetAsync            
            var parameter = Expression.Parameter(entityType, "e");
            var property = Expression.Property(parameter, propertyName);
            var constant = Expression.Constant(propertyValue);
            var body = Expression.Equal(property, constant);
            var predicate = Expression.Lambda(body, parameter);

            // 获取 GetAsync 方法信息
            MethodInfo getAsyncMethod = repository.GetType().GetMethod("GetAsync", new[] { typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(entityType, typeof(bool))), typeof(bool), typeof(CancellationToken) });

            if (getAsyncMethod == null)
            {
                throw new InvalidOperationException("Can't find GetAsync method");
            }

            // 调用 GetAsync 方法
            dynamic task = getAsyncMethod.Invoke(repository, new object[] { predicate, true, CancellationToken.None });
            task.Wait();
            var taskResult = task.Result;

            // 获取到的结果
            var resultProperty = taskResult.GetType().GetProperty("Id");
            var propertyId = resultProperty?.GetValue(taskResult);
            return (Guid)propertyId;
        }
        private async Task SetExportDtoCustomAttributeAync(List<TExportInput> dtos, List<TEntity> entities)
        {
            //foreach (var dto in dtos)
            for (int i = 0; i < dtos.Count; i++)
            {
                var dto = dtos[i];
                var entity = entities[i];
                var dtoProperties = typeof(TExportInput).GetProperties();
                //string name = CommonHelper.GetPropertyValueByName(dto, UFEConsts.ModelSearchField).ToString();
                //var entity = await GetEntityByNameAsync(name, dto, Repository);

                foreach (var dtoProp in dtoProperties)
                {
                    var exportAttr = dtoProp.GetCustomAttribute<ExportAttribute>();
                    if (exportAttr != null)
                    {

                        var relatedDomain = exportAttr.RelatedDomain;
                        var entityProp = typeof(TEntity).GetProperty(exportAttr.RelatedId);
                        var entityValue = entityProp.GetValue(entity);

                        var relatedValue = await GetEntityPropertyAsync(relatedDomain, (Guid)entityValue, exportAttr.RelatedProperty);

                        dtoProp.SetValue(dto, relatedValue);
                    }
                }
            }
        }
        private async Task<string> GetEntityPropertyAsync(string entityName, Guid entityId, string propertyName)
        {
            // 动态获取领域实体的类型
            Type entityType = Type.GetType(entityName);

            if (entityType == null)
            {
                throw new ArgumentException($"Can't find the type '{entityName}'");
            }

            // 构造 IRepository<TEntity, Guid> 类型
            Type repositoryType = typeof(IRepository<,>).MakeGenericType(entityType, typeof(Guid));

            // 获取 IRepository 实例
            var repository = LazyServiceProvider.GetRequiredService(repositoryType);

            if (repository == null)
            {
                throw new InvalidOperationException($"can't find repository: '{repositoryType.FullName}'");
            }

            // 使用反射调用 GetAsync 方法GetAsync            
            var methodInfo = repository.GetType().GetMethod("GetAsync", new Type[] { typeof(Guid), typeof(bool), typeof(CancellationToken) });

            if (methodInfo == null)
            {
                throw new InvalidOperationException("undefine GetAsync method");
            }

            // 调用 GetAsync 方法获取实体
            dynamic task = methodInfo.Invoke(repository, new object[] { entityId, false, CancellationToken.None });
            task.Wait();
            var taskResult = task.Result;

            // 获取到的结果
            var resultProperty = taskResult.GetType().GetProperty(propertyName);
            var propertyValue = resultProperty?.GetValue(taskResult).ToString();
            return propertyValue;
        }
        #endregion
    }
}
