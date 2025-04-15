using System;
using System.Linq;
using System.Threading.Tasks;
using EtymoLex.Academy.EntityFrameworkCore;
using EtymoLex.Academy.Repositories;
using Volo.Abp.EntityFrameworkCore;

namespace EtymoLex.Academy;

public class MorphemeRepository : NameObjectRepository <Morpheme, Guid>, IMorphemeRepository
{
    public MorphemeRepository(IDbContextProvider<AcademyDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<Morpheme>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}