using Volo.Abp.Modularity;

namespace EtymoLex.Academy;

[DependsOn(
    typeof(AcademyApplicationModule),
    typeof(AcademyDomainTestModule)
)]
public class AcademyApplicationTestModule : AbpModule
{

}
