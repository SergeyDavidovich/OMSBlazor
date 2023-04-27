using OMSBlazor.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace OMSBlazor.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class OMSBlazorController : AbpControllerBase
{
    protected OMSBlazorController()
    {
        LocalizationResource = typeof(OMSBlazorResource);
    }
}
