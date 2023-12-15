using ATBasketRobotServerAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
namespace ATBasketRobotServerAPI.Configurations;
public class PresentationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ExceptionMiddleware>();
        services.AddCors(options => options.AddDefaultPolicy(options =>
        {
            options.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(options => true);
        }));
        services.AddControllers()
            .AddApplicationPart(typeof(ATBasketRobotServer.Presentation.AssemblyReference).Assembly);
        // Lsearn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(setup =>
        {
            var jwtSecuritySheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);
            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecuritySheme, Array.Empty<string>() }
            });
        });
    }
}