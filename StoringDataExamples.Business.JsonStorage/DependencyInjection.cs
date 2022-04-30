using Microsoft.Extensions.DependencyInjection;
using StoringDataExamples.Business.Interfaces;
using StoringDataExamples.Business.JsonStorage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoringDataExamples.Business.JsonStorage
{
    public static class DependencyInjection
    {
        public static IServiceCollection UseJsonStorage(this IServiceCollection services)
        {
            services.AddSingleton<JsonRepository>();
            services.AddTransient<IRepository, JsonRepositoryService>();

            return services;
        }
    }
}
