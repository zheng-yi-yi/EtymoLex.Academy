using EtymoLex.Academy.Samples;
using Xunit;

namespace EtymoLex.Academy.EntityFrameworkCore.Applications;

[Collection(AcademyTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<AcademyEntityFrameworkCoreTestModule>
{

}
