using System;
using Volo.Abp.Domain.Repositories;

namespace EtymoLex.Academy.Repositories
{
    public interface IMorphemeExampleRepository : IRepository<MorphemeExample, Guid>
    {
    }
}
