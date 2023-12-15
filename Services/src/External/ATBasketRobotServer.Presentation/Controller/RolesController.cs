using ATBasketRobotServer.Application.Features.AppFeatures.RoleFeatures.Commands.CreateAllRoles;
using ATBasketRobotServer.Application.Features.AppFeatures.RoleFeatures.Commands.CreateRole;
using ATBasketRobotServer.Application.Features.AppFeatures.RoleFeatures.Commands.DeleteRole;
using ATBasketRobotServer.Application.Features.AppFeatures.RoleFeatures.Commands.UpdateRole;
using ATBasketRobotServer.Application.Features.AppFeatures.RoleFeatures.Queries.GetAllRoles;
using ATBasketRobotServer.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ATBasketRobotServer.Presentation.Controller;
[Authorize(AuthenticationSchemes = "Bearer")]

public class RolesController : ApiController
{
    public RolesController(IMediator mediator) : base(mediator)
    {
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateRole(CreateRoleCommand request)
    {
        CreateRoleCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetlAllRoles()
    {
        GetAllRolesQuery request = new();
        GetAllRolesQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateRole(UpdateRoleCommand request)
    {
        UpdateRoleCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        DeleteRoleCommand request = new(id);
        DeleteRoleCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> CreateAllRoles()
    {
        CreateStaticRolesCommand request = new();
        CreateStaticRolesCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}