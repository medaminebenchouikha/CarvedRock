using CarvedRock.OrderProcessor.Repository;
using Serilog;
using Serilog.Events;
using System.Data;
using System.Data.SqlClient;

namespace CarvedRock.OrderProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var name = typeof(Program).Assembly.GetName().Name;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", name)
                .WriteTo.Seq(serverUrl: "http://host.docker.internal:5341")
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.ForContext("Args", args)
                    .Information("Starting host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSingleton<IDbConnection>(d =>
                        new SqlConnection(hostContext.Configuration.GetConnectionString("Db")));
                        services.AddSingleton<IInventoryRepository, InventoryRepository>();
                        services.AddHostedService<Worker>();
                    })
                    .UseSerilog();
    }
}
