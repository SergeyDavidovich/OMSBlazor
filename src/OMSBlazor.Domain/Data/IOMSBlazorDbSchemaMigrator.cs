using System.Threading.Tasks;

namespace OMSBlazor.Data;

public interface IOMSBlazorDbSchemaMigrator
{
    Task MigrateAsync();
}
