using ATBasketRobotServer.Application.Features.AppFeatures.AuthFeatures.Commands.ChangePassword;
using ATBasketRobotServer.Application.Features.AppFeatures.AuthFeatures.Commands.CreateUser;
using ATBasketRobotServer.Application.Features.AppFeatures.AuthFeatures.Commands.GetTokenByRefreshToken;
using ATBasketRobotServer.Application.Features.AppFeatures.AuthFeatures.Commands.Login;
using ATBasketRobotServer.Application.Features.AppFeatures.AuthFeatures.Queries.GetAllUser;
using ATBasketRobotServer.Application.Features.AppFeatures.AuthFeatures.Queries.GetMainRolesByUserId;
using ATBasketRobotServer.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace ATBasketRobotServer.Presentation.Controller;
public class AuthController : ApiController
{
    private readonly ILogger<AuthController> _logger;
    public AuthController(IMediator mediator, ILogger<AuthController> logger) : base(mediator)
    {
        _logger = logger;
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginCommand request)
    {
        LoginCommandResponse response = await _mediator.Send(request);
        _logger.LogInformation(response.NameLastName + " User Login!");
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> GetTokenByRefreshToken(GetTokenByRefreshTokenCommand request)
    {
        GetTokenByRefreshTokenCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand request)
    {
        ChangePasswordCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    //[HttpPost("[action]")]
    //public async Task<IActionResult> GetRolesByUserIdAndCompanyId(GetRolesByUserIdAndCompanyIdQuery request)
    //{
    //    GetRolesByUserIdAndCompanyIdQueryResponse response = await _mediator.Send(request);
    //    return Ok(response);
    //}
    [HttpPost("[action]")]
    public async Task<IActionResult> GetMainRolesByUserId(GetMainRolesByUserIdQuery request)
    {
        GetMainRolesByUserIdQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateUser(CreateUserCommand request)
    {
        CreateUserCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllUser()
    {
        _logger.LogInformation("Get all User");
        GetAllUserQuery request = new();
        GetAllUserQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}