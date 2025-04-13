using Volo.Abp.Modularity;

namespace EtymoLex.Academy;

/* Inherit from this class for your domain layer tests. */
public abstract class AcademyDomainTestBase<TStartupModule> : AcademyTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
