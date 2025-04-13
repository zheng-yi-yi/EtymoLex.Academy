using EtymoLex.Academy.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace EtymoLex.Academy.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AcademyEntityFrameworkCoreModule),
    typeof(AcademyApplicationContractsModule)
)]
public class AcademyDbMigratorModule : AbpModule
{
}
