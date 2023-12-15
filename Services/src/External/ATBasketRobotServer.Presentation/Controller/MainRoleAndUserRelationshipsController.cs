using ATBasketRobotServer.Application.Features.AppFeatures.MainRoleAndUserRLFeatures.Commands.CreateMainRoleAndUserRL;
using ATBasketRobotServer.Application.Features.AppFeatures.MainRoleAndUserRLFeatures.Commands.RemoveByIdMainRoleAndUserRL;
using ATBasketRobotServer.Application.Features.AppFeatures.MainRoleAndUserRLFeatures.Queries;
using ATBasketRobotServer.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ATBasketRobotServer.Presentation.Controller;
public class MainRoleAndUserRelationshipsController : ApiController
{
    public MainRoleAndUserRelationshipsController(IMediator mediator) : base(mediator) { }
    [HttpPost("[action]")]
    public async Task<IActionResult> Create(CreateMainRoleAndUserRLCommand request)
    {
        CreateMainRoleAndUserRLCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> RevmoveById(RemoveByIdMainRoleAndUserRLCommand request)
    {
        RemoveByIdMainRoleAndUserRLCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        GetAllMainRoleAndUserQuery request = new();
        GetAllMainRoleAndUserQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}