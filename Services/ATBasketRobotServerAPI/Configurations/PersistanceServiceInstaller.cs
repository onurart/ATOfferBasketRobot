using ATBasketRobotServer.Domain.AppEntities.Identity;
using ATBasketRobotServer.Persistance;
using ATBasketRobotServer.Persistance.Context;
using Microsoft.EntityFrameworkCore;
namespace ATBasketRobotServerAPI.Configurations;
public class PersistanceServiceInstaller : IServiceInstaller
{
    private const string SectionName = "SqlServer";
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString(SectionName);
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>();
        services.AddAutoMapper(typeof(AssemblyReference).Assembly);
    }
}