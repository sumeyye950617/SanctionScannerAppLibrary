using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SanctionScannerAppLibrary;
using Serilog;

namespace SanctionScannerAppLibrary
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Debug(Serilog.Events.LogEventLevel.Information)
                .WriteTo.File("Logs.txt")
                .CreateLogger();
            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); // Startup s?n?f?n? burada belirtin
                });
    }
}