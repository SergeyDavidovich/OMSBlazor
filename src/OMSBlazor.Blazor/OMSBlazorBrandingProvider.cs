﻿using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace OMSBlazor.Blazor;

[Dependency(ReplaceServices = true)]
public class OMSBlazorBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "OMSBlazor";
}
