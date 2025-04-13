using Volo.Abp.Settings;

namespace EtymoLex.Academy.Settings;

public class AcademySettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AcademySettings.MySetting1));
    }
}
