using EtymoLex.Academy.Samples;
using Xunit;

namespace EtymoLex.Academy.EntityFrameworkCore.Domains;

[Collection(AcademyTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<AcademyEntityFrameworkCoreTestModule>
{

}
