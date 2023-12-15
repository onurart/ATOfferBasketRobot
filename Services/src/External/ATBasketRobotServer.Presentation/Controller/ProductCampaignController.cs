using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaign;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaignAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaignCompany;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.RemoveByIdProductCampaign;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.UpdateProductCampaign;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Queries.GetAllProductCampaign;
using ATBasketRobotServer.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ATBasketRobotServer.Presentation.Controller;
public class ProductCampaignController : ApiController
{
    public ProductCampaignController(IMediator mediator) : base(mediator)
    {
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateProductCampaign(CreateProductCampaignCommand request, CancellationToken cancellationToken)
    {
        CreateProductCampaignCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [AllowAnonymous]
    [HttpGet("[action]")]
    public async Task<IActionResult> CreateProductCampaignAll(CancellationToken cancellationToken)
    {
        CreateProductCampaignAllCommand request = new();
        CreateProductCampaignAllCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> CreateProductCampaignCompany(string companyid, CancellationToken cancellationToken)
    {
        CreateProductCampaignCompanyCommand request = new(companyid);
        CreateProductCampaignCompanyCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateProductCampaignStatus(UpdateProductCampaignCommand request, CancellationToken cancellationToken)
    {
        UpdateProductCampaignCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetAllProductCampaignStatus(string companyid)
    {
        GetAllProductCampaignQuery request = new(companyid);
        GetAllProductCampaignQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> RemoveByIdProductCampaignStatus(RemoveByIdProductCampaignCommand request, CancellationToken cancellationToken)
    {
        RemoveByIdProductCampaignCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}