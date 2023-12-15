using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOffer;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferAlghotim;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferCompany;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.RemoveByIdOffer;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.UpdateOffer;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.UpdateOfferUpdateDate;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Queries.GetAllOffer;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Queries.GetAllOfferDto;
using ATBasketRobotServer.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ATBasketRobotServer.Presentation.Controller;
public class OfferController : ApiController
{
    public OfferController(IMediator mediator) : base(mediator)
    {
    }    [HttpPost("[action]")]
    public async Task<IActionResult> CreateOffer(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        CreateOfferCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [AllowAnonymous]
    [HttpGet("[action]")]
    public async Task<IActionResult> CreateOfferAll(CancellationToken cancellationToken)
    {
        CreateOfferAllCommand request = new();
        CreateOfferAllCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> CreateOfferCompany(string companyid, CancellationToken cancellationToken)
    {
        CreateOfferCompanyCommand request = new(companyid);
        CreateOfferCompanyCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> CreateOfferAlghotim(string companyid, CancellationToken cancellationToken)
    {
        CreateOfferAlghotimCommand request = new(companyid);
        CreateOfferAlghotimCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateOffer(UpdateOfferCommand request, CancellationToken cancellationToken)
    {
        UpdateOfferCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateOfferUpdateDate(UpdateOfferUpdateDateCommand request, CancellationToken cancellationToken)
    {
        UpdateOfferUpdateDateCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetAllOffer(string companyid)
    {
        GetAllOfferQuery request = new(companyid);
        GetAllOfferQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{companyid}")]
    public async Task<IActionResult> GetAllOfferDto(string companyid)
    {
        GetAllOfferDtoQuery request = new(companyid);
        GetAllOfferDtoQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> RemoveByIdOffer(RemoveByIdOfferCommand request, CancellationToken cancellationToken)
    {
        RemoveByIdOfferCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}