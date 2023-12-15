using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Queries.GetAllProduct;
public sealed record GetAllProductQuery(string CompanyId) : IQuery<GetAllProductQueryResponse>;