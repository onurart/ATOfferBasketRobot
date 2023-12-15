using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatus;
using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatusAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatusCompany;
using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.RemoveByIdBasketStatus;
using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.UpdateBasketStatus;
using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Queries.GetAllBasketStatus;
using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Queries.GetAllBasketStatusDto;
using ATBasketRobotServer.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATBasketRobotServer.Presentation.Controller;
public class BasketStatusController : ApiController
{
    public BasketStatusController(IMediator mediator) : base(mediator)
    {
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateBasketStatus(CreateBasketStatusCommand request, CancellationToken cancellationToken)
    {
        CreateBasketStatusCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [AllowAnonymous]
    [HttpGet("[action]")]
    public async Task<IActionResult> CreateBasketStatusAll(CancellationToken cancellationToken)
    {
        CreateBasketStatusAllCommand request = new();
        CreateBasketStatusAllCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> CreateBasketStatusCompany(string companyid, CancellationToken cancellationToken)
    {
        CreateBasketStatusCompanyCommand request = new(companyid);
        CreateBasketStatusCompanyCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateBasketStatus(UpdateBasketStatusCommand request, CancellationToken cancellationToken)
    {
        UpdateBasketStatusCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetAllBasketStatus(string companyid)
    {
        GetAllBasketStatusQuery request = new(companyid);
        GetAllBasketStatusQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetAllBasketStatusDto(string companyid)
    {
        GetAllBasketStatusDtoQuery request = new(companyid);
        GetAllBasketStatusDtoQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> RemoveByIdBasketStatus(RemoveByIdBasketStatusCommand request, CancellationToken cancellationToken)
    {
        RemoveByIdBasketStatusCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}