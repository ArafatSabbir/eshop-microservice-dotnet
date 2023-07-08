using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions;

public static class HostExtensions
{
    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder,
        int? retry = 0) where TContext : DbContext
    {
        int retryForAvailability = retry.Value;

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(DbContext));
                InvokeSeeder(seeder, context, services);
                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(DbContext));
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occured while migrating the database used on context {DbContext}", typeof(DbContext));

                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    System.Threading.Thread.Sleep(2000);
                    MigrateDatabase<TContext>(host, seeder, retryForAvailability);
                }
            }
        }

        return host;
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
        IServiceProvider service) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, service);
    }
}