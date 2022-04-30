using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoringDataExamples.Business.Interfaces;
using StoringDataExamples.Business.SQLStorage.Persistence;
using StoringDataExamples.Business.SQLStorage.Services;

namespace StoringDataExamples.Business.SQLStorage
{
    public static class DependencyInjection
    {
        public static IServiceCollection UseSQLStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(connectionString,
                            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddTransient<IRepository, SQLRepositoryService>();

            return services;
        }
    }
}