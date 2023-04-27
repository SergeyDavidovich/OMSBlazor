using OMSBlazor.Localization;
using Volo.Abp.AspNetCore.Components;

namespace OMSBlazor.Blazor;

public abstract class OMSBlazorComponentBase : AbpComponentBase
{
    protected OMSBlazorComponentBase()
    {
        LocalizationResource = typeof(OMSBlazorResource);
    }
}
