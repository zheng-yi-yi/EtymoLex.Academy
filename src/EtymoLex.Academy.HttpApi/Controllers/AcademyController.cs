using EtymoLex.Academy.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EtymoLex.Academy.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AcademyController : AbpControllerBase
{
    protected AcademyController()
    {
        LocalizationResource = typeof(AcademyResource);
    }
}
