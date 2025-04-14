using System.Collections.Generic;

namespace EtymoLex.Academy.Dtos
{
    public class ImportResultDto
    {
        public bool Status { get; set; }
        public int TotalCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public List<ImportResultItemDto> Items { get; set; } = new List<ImportResultItemDto>();

    }
}
