using StripeModule.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace StripeModule.Permissions;

public class StripeModulePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(StripeModulePermissions.GroupName, L("Permission:StripeModule"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<StripeModuleResource>(name);
    }
}
