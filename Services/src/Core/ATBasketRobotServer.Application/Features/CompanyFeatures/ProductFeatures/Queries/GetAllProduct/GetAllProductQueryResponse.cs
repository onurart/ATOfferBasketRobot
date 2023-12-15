using ATBasketRobotServer.Domain.CompanyEntities;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Queries.GetAllProduct;
public sealed record GetAllProductQueryResponse(IList<Product> Data);