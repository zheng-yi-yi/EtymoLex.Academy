using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp;

namespace EtymoLex.Academy
{
    public abstract class NameObject<Tkey> : NameObjectNonTenant<Tkey>, IMultiTenant, ISoftDelete
    {
        protected NameObject(Tkey id) : base(id) { }

        protected NameObject() { }

        public Guid? TenantId { get; set; } = Guid.Empty;
        [NotMapped]
        public string TenantName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }


    }
    [Index(nameof(NormalizedName))]
    [Index(nameof(Name))]
    public abstract class NameObjectNonTenant<Tkey> : AuditedAggregateRoot<Tkey>
    {
        protected NameObjectNonTenant(Tkey id) : base(id) { }

        protected NameObjectNonTenant() { }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public string DisplayName { get; set; }

        public string NormalizedName { get; set; }
    }
}
