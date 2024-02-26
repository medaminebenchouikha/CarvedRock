using CarvedRock.Api;
using CarvedRock.Api.Domain;
using CarvedRock.Api.Interfaces;
using CarvedRock.Api.Middleware;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;


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
                   .Information("Starting web host");
    var builder = WebApplication.CreateBuilder(args);

    /*  var connectionString = "hello";
      var simpleProperty = "hey";
      var nestedProp = "here we go";*/

    var connectionString = builder.Configuration.GetConnectionString("Db");
    var simpleProperty = builder.Configuration.GetValue<string>("SimpleProperty");
    var nestedProp = builder.Configuration.GetValue<string>("Inventory:NestedProperty");

    Log.ForContext("connectionString", connectionString)
        .ForContext("simpleProperty", simpleProperty)
        .ForContext("nestedProp", nestedProp)
        .Information("Loaded configuration!", connectionString);

    var dbgView = (builder.Configuration as IConfigurationRoot).GetDebugView();
    Log.ForContext("ConfigurationDebug", dbgView)
        .Information("Configuration dump");
   

    // Add services to the container.

    //Add support to logging with SERILOG
    builder.Host.UseSerilog();

    builder.Services.AddScoped<IProductLogic, ProductLogic>();
    builder.Services.AddScoped<IQuickOrderLogic, QuickOrderLogic>();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "CarvedRock.Api",
            Version = "v1",
            Description = "The API for "
        });
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseMiddleware<CustomExceptionHandlingMiddleware>();

    //Add support to logging request with SERILOG
    //app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCustomRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
