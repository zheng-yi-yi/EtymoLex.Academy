using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace EtymoLex.Academy.Data;

/* This is used if database provider does't define
 * IAcademyDbSchemaMigrator implementation.
 */
public class NullAcademyDbSchemaMigrator : IAcademyDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
