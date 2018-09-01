using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using Pizzaria.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzaria.Code
{
    public static class ServiceProviderFactory
    {
        public static IServiceProvider ServiceProvider { get; }

        static ServiceProviderFactory()
        {
            HostingEnvironment env = new HostingEnvironment
            {
                ContentRootPath = Directory.GetCurrentDirectory(),
                EnvironmentName = "Development"
            };

            Startup startup = new Startup(env);
            ServiceCollection sc = new ServiceCollection();
            startup.ConfigureServices(sc);
            ServiceProvider = sc.BuildServiceProvider();

            
        }

        public static ApplicationDbContext GetApplicationDbContext()
        {
            var serviceScope = ServiceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            ApplicationDbContext applicationDbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            return applicationDbContext;
        }

    }
}
