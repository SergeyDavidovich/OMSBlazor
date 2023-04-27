using System;
using System.Collections.Generic;
using System.Text;
using OMSBlazor.Localization;
using Volo.Abp.Application.Services;

namespace OMSBlazor;

/* Inherit your application services from this class.
 */
public abstract class OMSBlazorAppService : ApplicationService
{
    protected OMSBlazorAppService()
    {
        LocalizationResource = typeof(OMSBlazorResource);
    }
}
