using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocument;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocumentAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocumentCompany;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.RemoveByIdDocument;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.UpdateDocument;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Queries.GetAllDocument;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Queries.GetAllDocumentDto;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Queries.GetDocOffCustTotal;
using ATBasketRobotServer.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ATBasketRobotServer.Presentation.Controller;
public class DocumentController : ApiController
{
    public DocumentController(IMediator mediator) : base(mediator)
    {
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateDocument(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        CreateDocumentCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [AllowAnonymous]
    [HttpGet("[action]")]
    public async Task<IActionResult> CreateDocumentAll(CancellationToken cancellationToken)
    {
        CreateDocumentAllCommand request = new();
        CreateDocumentAllCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> CreateDocumentCompany(string companyid, CancellationToken cancellationToken)
    {
        CreateDocumentCompanyCommand request = new(companyid);
        CreateDocumentCompanyCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateDocument(UpdateDocumentCommand request, CancellationToken cancellationToken)
    {
        UpdateDocumentCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetAllDocument(string companyid)
    {
        GetAllDocumentQuery request = new(companyid);
        GetAllDocumentQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetAllDocumentDto(string companyid)
    {
        GetAllDocumentDtoQuery request = new(companyid);
        GetAllDocumentDtoQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetDocumentDto(string companyid)
    {
        GetDocOffCustTotalQuery request = new(companyid);
        GetDocOffCustTotalQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> RemoveByIdDocument(RemoveByIdDocumentCommand request, CancellationToken cancellationToken)
    {
        RemoveByIdDocumentCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}