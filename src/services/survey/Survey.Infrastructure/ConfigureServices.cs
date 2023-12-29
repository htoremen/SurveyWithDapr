using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Survey.Shared.Common.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddDbContext(appSettings);

            return services;
        }

        private static void AddDbContext(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    appSettings.ConnectionStrings.ConnectionString,
                    configure =>
                    {
                        configure.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        configure.EnableRetryOnFailure();
                    }), ServiceLifetime.Scoped);

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        }
    }
}