using ATBasketRobotServer.Application.Features.CompanyFeatures.LogFeatures.Queires.GetLogsByTableName;
using ATBasketRobotServer.Presentation.Abstraction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ATBasketRobotServer.Presentation.Controller;
public class LogsController : ApiController
{
    public LogsController(IMediator mediator) : base(mediator) { }

    [HttpPost("[action]")]
    public async Task<IActionResult> GetLogsByTableName(GetLogsByTableNameQuery request)
    {
        GetLogsByTableNameQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}