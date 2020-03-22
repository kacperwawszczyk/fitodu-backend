using System;
using System.IO;
using Fitodu.Model;
using Fitodu.Model.Migrations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace Fitodu.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            Logger logger;

            using (var scope = host.Services.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetService<IConfiguration>();

                GlobalDiagnosticsContext.Set("configDir", $"{Directory.GetCurrentDirectory()}/Logs/");
                GlobalDiagnosticsContext.Set("connectionString", configuration.GetConnectionString("LogConnection"));

                logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

                try
                {
                    var context = scope.ServiceProvider.GetService<Context>();
                    Configuration.Initialize(context);
                }
                catch (Exception ex)
                {
                    logger.Log(NLog.LogLevel.Fatal, ex);
                }
            }

            try
            {
                host.Run();
            }
            catch (Exception ex)
            {
                logger.Log(NLog.LogLevel.Fatal, ex);
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", true, true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .UseStartup<Startup>()
                .UseNLog();
    }
}