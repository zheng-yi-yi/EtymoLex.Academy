using Volo.Abp.Identity;

namespace EtymoLex.Academy;

public static class AcademyConsts
{
    public const string DbTablePrefix = "EtymoLex_";
    public const string? DbSchema = null;
    public const string AdminEmailDefaultValue = IdentityDataSeedContributor.AdminEmailDefaultValue;
    public const string AdminPasswordDefaultValue = IdentityDataSeedContributor.AdminPasswordDefaultValue;
}
