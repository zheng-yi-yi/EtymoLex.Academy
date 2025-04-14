using Volo.Abp.Application.Dtos;

namespace EtymoLex.Academy.Dtos;

public class ModelingInputDto<TKey> : PagedAndSortedResultRequestDto
{
    public TKey Id { get; set; }
}
