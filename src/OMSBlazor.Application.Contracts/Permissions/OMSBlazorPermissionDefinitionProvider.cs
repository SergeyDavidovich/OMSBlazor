using OMSBlazor.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace OMSBlazor.Permissions;

public class OMSBlazorPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(OMSBlazorPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(OMSBlazorPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OMSBlazorResource>(name);
    }
}
