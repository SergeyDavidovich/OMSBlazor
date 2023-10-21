using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Modularity;

namespace OMSBlazor.NotificationSender
{
    [DependsOn(typeof(AbpAspNetCoreSignalRModule))]
    public class OMSBlazorNotificationSenderModule : AbpModule
    {

    }
}