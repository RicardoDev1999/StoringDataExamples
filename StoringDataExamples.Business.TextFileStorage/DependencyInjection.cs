using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoringDataExamples.Business.Interfaces;
using StoringDataExamples.Business.TextFileStorage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoringDataExamples.Business.TextFileStorage
{
    public static class DependencyInjection
    {
        public static IServiceCollection UseTextFileStorage(this IServiceCollection services) 
        {
            services.AddSingleton<TextFileRepository>();
            services.AddTransient<IRepository, TextFileRepositoryService>();

            return services;
        }
    }
}
