using Xunit;

namespace EtymoLex.Academy.EntityFrameworkCore;

[CollectionDefinition(AcademyTestConsts.CollectionDefinitionName)]
public class AcademyEntityFrameworkCoreCollection : ICollectionFixture<AcademyEntityFrameworkCoreFixture>
{

}
