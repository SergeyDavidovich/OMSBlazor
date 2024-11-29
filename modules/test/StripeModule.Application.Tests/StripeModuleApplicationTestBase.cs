using Volo.Abp.Modularity;

namespace StripeModule;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class StripeModuleApplicationTestBase<TStartupModule> : StripeModuleTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
