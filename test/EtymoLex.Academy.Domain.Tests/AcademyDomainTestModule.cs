using Volo.Abp.Modularity;

namespace EtymoLex.Academy;

[DependsOn(
    typeof(AcademyDomainModule),
    typeof(AcademyTestBaseModule)
)]
public class AcademyDomainTestModule : AbpModule
{

}
