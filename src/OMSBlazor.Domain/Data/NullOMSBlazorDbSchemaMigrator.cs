using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace OMSBlazor.Data;

/* This is used if database provider does't define
 * IOMSBlazorDbSchemaMigrator implementation.
 */
public class NullOMSBlazorDbSchemaMigrator : IOMSBlazorDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
