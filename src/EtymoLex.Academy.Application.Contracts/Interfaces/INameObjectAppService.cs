using EtymoLex.Academy.Dtos;
using EtymoLex.Academy.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EtymoLex.Academy.Interfaces
{
    public interface INameObjectAppService<TEntityDto, TKey, TGetListInput, TCreateOrUpdateInput, TExportDto> : ICrudAppService<TEntityDto, TKey, TGetListInput, TCreateOrUpdateInput>
            where TEntityDto : NameObjectDto<TKey>
    {
        Task<ImportResultDto> Import(IEnumerable<TExportDto> entities, OverridingMode mode);
        Task<byte[]> Export(FileType fileType, IEnumerable<TKey> ids);
        Task<byte[]> ExportAll(FileType fileType);
        Task<bool> MultipleDelete(IEnumerable<TKey> ids);
        Task<PagedResultDto<ModelingHistoryDto>> GetModelingHistory(ModelingInputDto<TKey> input);
        Task<TEntityDto> CreateOrUpdateAsync(TCreateOrUpdateInput data);
        Task<TEntityDto?> GetByNameAsync(string name);
        Task<IEnumerable<TEntityDto>> GetAllInstancesAsync();
        Task<IEnumerable<TEntityDto>> MultipleUpdateAsync(Dictionary<TKey, TCreateOrUpdateInput> inputs);
        Task<TEntityDto> CopyAsync(TCreateOrUpdateInput input);
        Task<IEnumerable<TEntityDto>> GetExistInstancesAsync(IEnumerable<TExportDto> entities);
    }
}
