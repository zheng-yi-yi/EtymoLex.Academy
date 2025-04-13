using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EtymoLex.Academy.Data;
using Volo.Abp.DependencyInjection;

namespace EtymoLex.Academy.EntityFrameworkCore;

public class EntityFrameworkCoreAcademyDbSchemaMigrator
    : IAcademyDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreAcademyDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the AcademyDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<AcademyDbContext>()
            .Database
            .MigrateAsync();
    }
}
