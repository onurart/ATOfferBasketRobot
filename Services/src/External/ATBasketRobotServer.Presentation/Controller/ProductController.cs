using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.CreateProduct;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.CreateProductAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.CreateProductCompany;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.RemoveByIdProduct;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.UpdateProduct;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.UpdateProductIsActive;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Queries.GetAllProduct;
using ATBasketRobotServer.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ATBasketRobotServer.Presentation.Controller;
//[Authorize(AuthenticationSchemes = "Bearer")]
public class ProductController : ApiController
{
    public ProductController(IMediator mediator) : base(mediator) { }
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateProduct(CreateProductCommand request, CancellationToken cancellationToken)
    {
        CreateProductCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> CreateProductAll(CancellationToken cancellationToken)
    {
        CreateProductAllCommand request = new();
        CreateProductAllCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [AllowAnonymous]
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> CreateProductCompany(string companyid, CancellationToken cancellationToken)
    {
        CreateProductCompanyCommand request = new(companyid);
        CreateProductCompanyCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateProduct(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        UpdateProductCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateProductIsActive(UpdateProductIsActiveCommand request, CancellationToken cancellationToken)
    {
        UpdateProductIsActiveCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetAllProduct(string companyid)
    {
        GetAllProductQuery request = new(companyid);
        GetAllProductQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> RemoveByIdProduct(RemoveByIdProductCommand request, CancellationToken cancellationToken)
    {
        RemoveByIdProductCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}
