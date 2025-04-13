using System.Threading.Tasks;

namespace EtymoLex.Academy.Data;

public interface IAcademyDbSchemaMigrator
{
    Task MigrateAsync();
}
