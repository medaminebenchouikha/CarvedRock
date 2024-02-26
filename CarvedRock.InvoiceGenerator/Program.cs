// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CarvedRock.InvoiceGenerator
{
    class Program
    {
        private static IConfigurationRoot _config;

        static void Main(string[] args)
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

            ConfigureLogging();

            try
            {
                var connectionString = _config.GetConnectionString("Db");
                var simpleProperty = _config.GetValue<string>("SimpleProperty");
                var nestedProp = _config.GetValue<string>("Inventory:NestedProperty");

                Log.ForContext("connectionString", connectionString)
                    .ForContext("simpleProperty", simpleProperty)
                    .ForContext("nestedProp", nestedProp)
                    .Information("Loaded configuration!", connectionString);

                Log.ForContext("Args", args)
                    .Information("Starting host");

                Console.WriteLine("Hello World!");

                Log.Information("Finished execution!");
            }
            catch (Exception ex)
            {
                Log.Error("some kind of exception occured.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureLogging()
        {
            var name = typeof(Program).Assembly.GetName().Name;

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", name)
                .WriteTo.Seq(serverUrl: "http://host.docker.internal:5341")
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
