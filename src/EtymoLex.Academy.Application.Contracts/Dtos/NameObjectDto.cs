using System;
using EtymoLex.Academy.Attributes;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace EtymoLex.Academy.Dtos
{
    public abstract class NameObjectDto<Tkey> : ExtensibleAuditedEntityDto<Tkey>
    {
        public virtual string Name { get; set; }

        public virtual string? Description { get; set; }
        [Required]
        public virtual Guid? TenantId { get; set; }
        public virtual string TenantName { get; set; } = string.Empty;
        public string NormalizedName { get; set; }
        public virtual string DisplayName { get; set; }

        public virtual string Creator { get; set; }
        public virtual string LastModifier { get; set; }
    }
    public abstract class CreateUpdateNameObjectDto
    {
        [Required]
        [StringLength(400)]
        public virtual string Name { get; set; }

        public virtual string? Description { get; set; }

        public virtual Guid? TenantId { get; set; }
        public virtual string? TenantName { get; set; } = string.Empty;
        public virtual string DisplayName { get; set; }
        public ExtraPropertyDictionary? ExtraProperties { get; set; }
    }

    public abstract class GetNameObjectInput : PagedAndSortedResultRequestDto
    {
        public virtual string Filter { get; set; } = "%";
    }

    public class ExportNameObjectDto
    {
        [Required]
        public virtual string Name { get; set; }
        [Optional]
        public string? DisplayName { get; set; }
        public string? Description { get; set; } = string.Empty;

        public DateTime? CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
