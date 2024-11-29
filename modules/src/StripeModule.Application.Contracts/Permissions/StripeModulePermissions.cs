using Volo.Abp.Reflection;

namespace StripeModule.Permissions;

public class StripeModulePermissions
{
    public const string GroupName = "StripeModule";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(StripeModulePermissions));
    }
}
