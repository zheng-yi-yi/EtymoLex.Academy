using Microsoft.Extensions.Localization;
using EtymoLex.Academy.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace EtymoLex.Academy;

[Dependency(ReplaceServices = true)]
public class AcademyBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<AcademyResource> _localizer;

    public AcademyBrandingProvider(IStringLocalizer<AcademyResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
