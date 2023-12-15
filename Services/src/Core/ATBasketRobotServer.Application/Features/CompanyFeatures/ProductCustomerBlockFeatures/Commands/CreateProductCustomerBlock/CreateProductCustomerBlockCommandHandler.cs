using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCustomerBlockFeatures.Commands.CreateProductCustomerBlock;
public sealed class CreateProductCustomerBlockCommandHandler : ICommandHandler<CreateProductCustomerBlockCommand, CreateProductCustomerBlockCommandResponse>
{
    private readonly IProductCustomerBlockService _service;
    private readonly ILogService _logService;

    public CreateProductCustomerBlockCommandHandler(IProductCustomerBlockService service, ILogService logService)
    {
        _service = service;
        _logService = logService;
    }

    public async Task<CreateProductCustomerBlockCommandResponse> Handle(CreateProductCustomerBlockCommand request, CancellationToken cancellationToken)
    {
        ProductCustomerBlock entity = await _service.CreateProductCustomerBlockAsync(request, cancellationToken);
        return new CreateProductCustomerBlockCommandResponse();
    }
}