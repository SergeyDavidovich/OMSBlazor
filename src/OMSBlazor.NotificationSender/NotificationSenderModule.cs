using Volo.Abp.Modularity;
using Volo.Abp.AspNetCore.SignalR;

namespace OMSBlazor.NotificationSender
{
    [DependsOn(typeof(AbpAspNetCoreSignalRModule))]
    public class NotificationSenderModules : AbpModule
    {

    }
}