using ATBasketRobotServer.Application.Services.AppServices;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.CompanyRepositories;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.MainRoleAndUserRelationshipRepositories;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.MainRoleReporsitories;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.UserAndCompanyRelationshipRepositories;
using ATBasketRobotServer.Domain.Repositories.AppDbContext.UserRoleRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.BasketStatusRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.CustomerRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.DocumentRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.LogRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.OfferRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.ProductCampaignRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.ProductCustomerBlockRepositories;
using ATBasketRobotServer.Domain.Repositories.CompanyDbContext.ProductRepositories;
using ATBasketRobotServer.Domain.UnitOfWorks;
using ATBasketRobotServer.Persistance;
using ATBasketRobotServer.Persistance.Repositories.AppDbContext.CompanyRepositories;
using ATBasketRobotServer.Persistance.Repositories.AppDbContext.MainRoleAndUserRelationshipRepositories;
using ATBasketRobotServer.Persistance.Repositories.AppDbContext.MainRoleRepositories;
using ATBasketRobotServer.Persistance.Repositories.AppDbContext.UserAndCompanyRelationshipRepositories;
using ATBasketRobotServer.Persistance.Repositories.AppDbContext.UserRoleRepositories;
using ATBasketRobotServer.Persistance.Repositories.CompanyDbContext.BasketStatusRepositories;
using ATBasketRobotServer.Persistance.Repositories.CompanyDbContext.CustomerRepositories;
using ATBasketRobotServer.Persistance.Repositories.CompanyDbContext.DocumentRepositories;
using ATBasketRobotServer.Persistance.Repositories.CompanyDbContext.LogRepositories;
using ATBasketRobotServer.Persistance.Repositories.CompanyDbContext.OfferRepositories;
using ATBasketRobotServer.Persistance.Repositories.CompanyDbContext.ProductCampaignRepositories;
using ATBasketRobotServer.Persistance.Repositories.CompanyDbContext.ProductCustomerBlockRepositories;
using ATBasketRobotServer.Persistance.Repositories.CompanyDbContext.ProductRepositories;
using ATBasketRobotServer.Persistance.Services.AppServices;
using ATBasketRobotServer.Persistance.Services.CompanyServices;
using ATBasketRobotServer.Persistance.UnitOfWorks;
//UsingSpot
namespace ATBasketRobotServerAPI.Configurations;
public class PersistanceDIServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        #region Context UnitOfWork
        services.AddScoped<ICompanyDbUnitOfWork, CompanyDbUnitOfWork>();
        services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
        services.AddScoped<IContextService, ContextService>();
        #endregion

        #region Services
        #region CompanyDbContext
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IBasketStatusService, BasketStatusService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IProductCampaignService, ProductCampaignService>();
        services.AddScoped<IOfferService, OfferService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IProductCustomerBlockService, ProductCustomerBlockService>();
        //CompanyServiceDISpot
        #endregion

        #region AppDbContext
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IMainRoleService, MainRoleService>();
        services.AddScoped<IMainRoleAndUserRelationshipService, MainRoleAndUserRelationshipService>();
        services.AddScoped<IUserAndCompanyRelationshipService, UserAndCompanyRelationshipService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        //AppServiceDISpot
        #endregion
        #endregion

        #region Repositories
        #region CompanyDbContext
        services.AddScoped<ILogCommandRepository, LogCommandRepository>();
        services.AddScoped<ILogQueryRepository, LogQueryRepository>();
        services.AddScoped<IProductCommandRepository, ProductCommandRepository>();
        services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
        services.AddScoped<IBasketStatusCommandRepository, BasketStatusCommandRepository>();
        services.AddScoped<IBasketStatusQueryRepository, BasketStatusQueryRepository>();
        services.AddScoped<ICustomerCommandRepository, CustomerCommandRepository>();
        services.AddScoped<ICustomerQueryRepository, CustomerQueryRepository>();
        services.AddScoped<IDocumentCommandRepository, DocumentCommandRepository>();
        services.AddScoped<IDocumentQueryRepository, DocumentQueryRepository>();
        services.AddScoped<IProductCampaignCommandRepository, ProductCampaignCommandRepository>();
        services.AddScoped<IProductCampaignQueryRepository, ProductCampaignQueryRepository>();
        services.AddScoped<IOfferCommandRepository, OfferCommandRepository>();
        services.AddScoped<IOfferQueryRepository, OfferQueryRepository>();
        services.AddScoped<IProductCustomerBlockCommandRepository, ProductCustomerBlockCommandRepository>();
        services.AddScoped<IProductCustomerBlockQueryRepository, ProductCustomerBlockQueryRepository>();
        services.AddHttpClient();
        //services.AddHttpClient<IProductService, ProductService>();
        //CompanyRepositoryDISpot
        #endregion


        #region AppDbContext
        services.AddScoped<ICompanyCommandRepository, CompanyCommandRepository>();
        services.AddScoped<ICompanyQueryRepository, CompanyQueryRepository>();
        services.AddScoped<IMainRoleCommandRepository, MainRoleCommandRepository>();
        services.AddScoped<IMainRoleQueryRepository, MainRoleQueryRepository>();
        services.AddScoped<IMainRoleAndUserRelationshipCommandRepository, MainRoleAndUserRelationshipCommandRepository>();
        services.AddScoped<IMainRoleAndUserRelationshipQueryRepository, MainRoleAndUserRelationshipQueryRepository>();
        services.AddScoped<IUserAndCompanyRelationshipCommandRepository, UserAndCompanyRelationshipCommandRepository>();
        services.AddScoped<IUserAndCompanyRelationshipQueryRepository, UserAndCompanyRelationshipQueryRepository>();
        services.AddScoped<IUserRoleCommandRepository, UserRoleCommandRepository>();
        services.AddScoped<IUserRoleQueryRepository, UserRoleQueryRepository>();
        //AppRepositoryDISpot
        #endregion
        #endregion
    }
}