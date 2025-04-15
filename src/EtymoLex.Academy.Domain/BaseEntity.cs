using System;
using Volo.Abp;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Domain.Entities.Auditing;


namespace EtymoLex.Academy
{
    public class BaseEntity<Tkey> : AuditedAggregateRoot<Tkey>, IMultiTenant, ISoftDelete
    {
        public Guid? TenantId { get; set; } = Guid.Empty;
        public bool IsDeleted { get; set; }
    }
}
