using ATBasketRobotServer.Application.Abstractions;
using ATBasketRobotServer.Application.Services;
using ATBasketRobotServer.Infrasturcture.Authentication;
using ATBasketRobotServer.Infrasturcture.Services;
namespace ATBasketRobotServerAPI.Configurations;
public class InfrustructureDIServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IRabbitMQService, RabbitMQService>();
        services.AddScoped<IApiService, ApiService>();
    }
}