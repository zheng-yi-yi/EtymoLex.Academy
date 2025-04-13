using Volo.Abp.Modularity;

namespace EtymoLex.Academy;

public abstract class AcademyApplicationTestBase<TStartupModule> : AcademyTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
