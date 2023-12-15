using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCustomerBlockFeatures.Commands.CreateProductCustomerBlock;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCustomerBlockFeatures.Commands.RemoveProductCustomerBlock;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCustomerBlockFeatures.Queries.GetAllProductCustomerBlock;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCustomerBlockFeatures.Queries.GetAllProductCustomerBlockDto;
using ATBasketRobotServer.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace ATBasketRobotServer.Presentation.Controller;
public class ProductCustomerBlockController : ApiController
{
    public ProductCustomerBlockController(IMediator mediator) : base(mediator) { }
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateProductCustomerBlock(CreateProductCustomerBlockCommand request, CancellationToken cancellationToken)
    {
        CreateProductCustomerBlockCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetAllProductCustomerBlock(string companyid)
    {
        GetAllProductCustomerBlockQuery request = new(companyid);
        GetAllProductCustomerBlockQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> RemoveProductCustomerBlock(RemoveProductCustomerBlockCommand request, CancellationToken cancellationToken)
    {
        RemoveProductCustomerBlockCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetAllProductCustomerBlockDto(string companyid)
    {
        GetAllProductCustomerBlockDtoQuery request = new(companyid);
        GetAllProductCustomerBlockDtoQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}