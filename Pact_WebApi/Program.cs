using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Pact_WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
                        config.AddJsonFile("appsettings.json");
                        config.AddEnvironmentVariables();
                    })
                .UseDefaultServiceProvider(options => options.ValidateScopes = false)
                .UseStartup<Startup>();
    }
}
