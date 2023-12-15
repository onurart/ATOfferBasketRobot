using ATBasketRobotServerAPI.OptionsSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace ATBasketRobotServerAPI.Configurations;
public class AuthenticationAndAuthorizationSeviceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        services.AddAuthorization();
    }
}