using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Queries.GetAllDocument;
public sealed record GetAllDocumentQuery(string CompanyId) : IQuery<GetAllDocumentQueryResponse>;