using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.AppFeatures.CompanyFeatures.Queries.GetAllCompany;
public sealed record GetAllCompanyQuery() : IQuery<GetAllCompanyQueryResponse>;