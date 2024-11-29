using Volo.Abp.Modularity;

namespace StripeModule;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class StripeModuleDomainTestBase<TStartupModule> : StripeModuleTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
