using Shouldly;
using System.Threading.Tasks;
using Xunit;
using Volo.Abp.Modularity;

namespace EtymoLex.Academy;

public class MorphemeAppServiceTests<TStartupModule> : AcademyApplicationTestBase<TStartupModule> where TStartupModule : IAbpModule
{
    private readonly IMorphemeAppService _morphemeAppService;

    public MorphemeAppServiceTests()
    {
        _morphemeAppService = GetRequiredService<IMorphemeAppService>();
    }

    /*
    [Fact]
    public async Task Test1()
    {
        // Arrange

        // Act

        // Assert
    }
    */
}

