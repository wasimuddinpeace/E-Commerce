using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DutchTreat
{ 
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            SeedDB(host);
            
            host.Run();
           //CreateHostBuilder(args).Build().Run();
        }

        private static void SeedDB(IWebHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
                seeder.SeedAsync().Wait();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration(SetupConfiguration)
                .UseStartup<Startup>()
            .Build();

        //private static void SetupConfiguration(WebHostBuilderContext arg1, IConfigurationBuilder arg2)
        //{
        //    throw new NotImplementedException();
        //}

        private static void SetupConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        {
            //Removing Default Configuration Options
            builder.Sources.Clear();
            //see the AddJson Method on Builder object
            builder.AddJsonFile("config.json",false,true);
            builder.AddEnvironmentVariables();
        }
    }
}
