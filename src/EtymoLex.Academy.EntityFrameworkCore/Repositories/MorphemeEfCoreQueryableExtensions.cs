using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EtymoLex.Academy;

public static class MorphemeEfCoreQueryableExtensions
{
    public static IQueryable<Morpheme> IncludeDetails(this IQueryable<Morpheme> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            // .Include(x => x.xxx) // TODO: AbpHelper generated
            ;
    }
}
