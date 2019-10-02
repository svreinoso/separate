using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Separate.Data;
using Separate.Data.Entities;
using Separate.Services;
using System;
using System.IO;

namespace Separate.AutoRun
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            IServiceCollection services = new ServiceCollection();
            Startup startup = new Startup();
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            
            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogDebug("Logger is working!");
            logger.LogError("Logger is working!");
            logger.LogInformation("Logger is working!");
            logger.LogWarning("Logger is working!");

            // Get Service and call method
            var service = serviceProvider.GetService<IEmailServices>();
            var a = service.GetBookmarksCount();
            Console.WriteLine(a);
        }
        
    }

    public class Startup
    {
        IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(Path.GetFullPath("../../../../Separate/appsettings.json"), optional: true)
                .AddJsonFile(Path.GetFullPath("../../../../Separate/appsettings.Development.json"), optional: true);
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    assembly => assembly.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });
            
            services.AddLogging(x => {
                x.AddConfiguration(Configuration.GetSection("Logging"));
                x.AddConsole();
                x.AddDebug();
            });

            services.AddSingleton(Configuration);
            services.AddSingleton<IEmailServices, EmailServices>();
        }
    }
}
