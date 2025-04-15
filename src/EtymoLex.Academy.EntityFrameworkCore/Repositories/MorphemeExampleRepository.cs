using EtymoLex.Academy.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EtymoLex.Academy.Repositories
{
    public class MorphemeExampleRepository : EfCoreRepository<AcademyDbContext, MorphemeExample, Guid>, IMorphemeExampleRepository
    {
        public MorphemeExampleRepository(IDbContextProvider<AcademyDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<MorphemeExample>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }
    }
}
