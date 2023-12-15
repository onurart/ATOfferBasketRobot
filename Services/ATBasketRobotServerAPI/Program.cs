using ATBasketRobotServer.Persistance.Context;
using ATBasketRobotServerAPI.Configurations;
using ATBasketRobotServerAPI.Middleware;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
try
{

    //Yeni Mac
    var builder = WebApplication.CreateBuilder(args);
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(LogLevel.Trace);
    builder.Host.UseNLog();
    builder.Services.InstallServices(builder.Configuration, typeof(IServiceInstaller).Assembly);
    builder.Host.UseWindowsService();
    // builder.WebHost.UseKestrel(options =>
    //     {
    //         var devOverride = Environment.GetEnvironmentVariable("Production");
    //         if (!string.IsNullOrWhiteSpace(devOverride))
    //         {
    //             options.ListenLocalhost(9200);
    //         }
    //         else
    //         {
    //             options.ListenAnyIP(9200);
    //         }
    //     });
    var app = builder.Build();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionMiddleware();
    app.UseHttpsRedirection();
    app.UseCors();
    app.MapControllers();





    using (var context = new AppDbContext()) { context.Database.Migrate(); }
    app.Run();



}
catch (Exception exception)
{
    //NLog: catch setup errors
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
