using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCustomerBlockFeatures.Commands.RemoveProductCustomerBlock;
public class RemoveProductCustomerBlockCommandHandler : ICommandHandler<RemoveProductCustomerBlockCommand, RemoveProductCustomerBlockCommandResponse>
{
    private readonly IProductCustomerBlockService _service;
    private readonly ILogService _logService;

    public RemoveProductCustomerBlockCommandHandler(IProductCustomerBlockService service, ILogService logService)
    {
        _service = service;
        _logService = logService;
    }

    public async Task<RemoveProductCustomerBlockCommandResponse> Handle(RemoveProductCustomerBlockCommand request, CancellationToken cancellationToken)
    {
        ProductCustomerBlock result = await _service.RemoveProductCustomerBlockAsync(request, cancellationToken);
        return new();
    }
}