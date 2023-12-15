using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocument;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocumentAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocumentCompany;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Dtos;
namespace ATBasketRobotServer.Application.Services.CompanyServices;
public interface IDocumentService
{
    Task<Document> CreateDocumentAsync(CreateDocumentCommand request, CancellationToken cancellationToken);
    Task CreateDocumentAllAsync(CreateDocumentAllCommand request, CancellationToken cancellationToken);
    Task CreateDocumentCompanyAsync(CreateDocumentCompanyCommand request, CancellationToken cancellationToken);
    Task<IList<Document>> GetAllAsync(string companyId);
    Task<IList<DocumentDto>> GetAllDtoAsync(string companyId);
    Task UpdateAsync(Document product, string companyId);
    Task<Document> RemoveByIdDocumentAsync(string id, string companyId);
    Task<Document> GetByDocumentCodeAsync(string companyId, string documentno, CancellationToken cancellationToken);
    Task<Document> GetByIdAsync(string id, string companyId);
    Task<IList<DocOffCustTotalDto>> GetDocOffCustTotalDtoAsync(string companyId);
}