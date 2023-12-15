using System.Collections;
using ATBasketRobotServer.Domain.Dtos.Report;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.ProductStock.GetProductOffSale;
public sealed record GetProductOffSaleQueryResponse(IList<ProductDto> data);