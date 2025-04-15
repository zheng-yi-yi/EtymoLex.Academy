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

        var MorphemeGroup = context.AddGroup(AcademyPermissions.Morpheme.Default, L("Morpheme"));
        var morphemePermission = MorphemeGroup.AddPermission(AcademyPermissions.Morpheme.Default, L("View"));
        morphemePermission.AddChild(AcademyPermissions.Morpheme.View, L("View"));
        morphemePermission.AddChild(AcademyPermissions.Morpheme.Create, L("Create"));
        morphemePermission.AddChild(AcademyPermissions.Morpheme.Edit, L("Edit"));
        morphemePermission.AddChild(AcademyPermissions.Morpheme.Copy, L("Copy"));
        morphemePermission.AddChild(AcademyPermissions.Morpheme.Import, L("Import"));
        morphemePermission.AddChild(AcademyPermissions.Morpheme.Export, L("Export"));
        morphemePermission.AddChild(AcademyPermissions.Morpheme.Delete, L("Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AcademyResource>(name);
    }
}
