using ATBasketRobotServer.Domain.AppEntities;
namespace ATBasketRobotServer.Application.Features.AppFeatures.CompanyFeatures.Queries.GetAllCompany;
public sealed record GetAllCompanyQueryResponse(List<Company> Companies);