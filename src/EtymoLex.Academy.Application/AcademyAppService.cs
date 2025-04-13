using EtymoLex.Academy.Localization;
using Volo.Abp.Application.Services;

namespace EtymoLex.Academy;

/* Inherit your application services from this class.
 */
public abstract class AcademyAppService : ApplicationService
{
    protected AcademyAppService()
    {
        LocalizationResource = typeof(AcademyResource);
    }
}
