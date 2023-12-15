using ATBasketRobotServer.Application.Features.AppFeatures.MainRoleFeatures.Commands.CreateMainRole;
using ATBasketRobotServer.Application.Features.AppFeatures.MainRoleFeatures.Commands.CreateRole;
using ATBasketRobotServer.Application.Features.AppFeatures.MainRoleFeatures.Commands.CreateStaticMainRoles;
using ATBasketRobotServer.Application.Features.AppFeatures.MainRoleFeatures.Commands.RemoveMainRole;
using ATBasketRobotServer.Application.Features.AppFeatures.MainRoleFeatures.Commands.UpdateMainRole;
using ATBasketRobotServer.Application.Features.AppFeatures.MainRoleFeatures.Queries.GetAllMainRole;
using ATBasketRobotServer.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ATBasketRobotServer.Presentation.Controller;
[Authorize(AuthenticationSchemes = "Bearer")]

public sealed class MainRolesController : ApiController
{
    public MainRolesController(IMediator mediator) : base(mediator)
    {
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Create(CreateMainRoleCommand request, CancellationToken cancellationToken)
    {
        CreateMainRoleCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateStaticMainRoles(CancellationToken cancellationToken)
    {
        CreateStaticMainRolesCommand request = new(null);
        CreateStaticMainRolesCommandResponse response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        GetAllMainRoleQuery request = new();
        GetAllMainRoleQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> RemoveById(RemoveByIdMainRoleCommand request)
    {
        RemoveByIdMainRoleCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Update(UpdateMainRoleCommand request)
    {
        UpdateMainRoleCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}