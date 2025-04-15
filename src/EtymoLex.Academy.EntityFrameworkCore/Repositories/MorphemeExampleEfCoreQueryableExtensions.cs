using System.Linq;

namespace EtymoLex.Academy.Repositories
{
    public static class MorphemeExampleEfCoreQueryableExtensions
    {
        public static IQueryable<MorphemeExample> IncludeDetails(this IQueryable<MorphemeExample> queryable, bool include = true)
        {
            if (!include)
            {
                return queryable;
            }

            return queryable
                ;
        }
    }
}
