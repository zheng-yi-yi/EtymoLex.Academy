using EtymoLex.Academy.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace EtymoLex.Academy.Permissions;

public class AcademyPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AcademyPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(AcademyPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AcademyResource>(name);
    }
}
