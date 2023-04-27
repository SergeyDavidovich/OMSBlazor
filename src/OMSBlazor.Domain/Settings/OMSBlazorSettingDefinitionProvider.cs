using Volo.Abp.Settings;

namespace OMSBlazor.Settings;

public class OMSBlazorSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(OMSBlazorSettings.MySetting1));
    }
}
