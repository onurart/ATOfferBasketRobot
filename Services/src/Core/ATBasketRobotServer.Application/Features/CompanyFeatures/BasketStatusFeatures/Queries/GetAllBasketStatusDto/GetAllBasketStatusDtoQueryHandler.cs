using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Dtos;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Queries.GetAllBasketStatusDto;
public sealed class GetAllBasketStatusDtoQueryHandler : IQueryHandler<GetAllBasketStatusDtoQuery, GetAllBasketStatusDtoQueryResponse>
{
    private readonly IBasketStatusService _service;
    public GetAllBasketStatusDtoQueryHandler(IBasketStatusService service)
    {
        _service = service;
    }

    public async Task<GetAllBasketStatusDtoQueryResponse> Handle(GetAllBasketStatusDtoQuery request, CancellationToken cancellationToken)
    {
       // return new(await _service.GetAllAsync(request.companyId));
       // var result = await _service.GetAllAsync(request.companyId);
        //var result = await _service.GetAll(request.companyId).Include(x=>x.Customer).Include(x=>x.Product).ToListAsync();
        var result = await _service.GetAllDtoAsync(request.companyId);
        //var result = await _service.GetAll(request.companyId).ToListAsync();
       // var res = _mapper.Map<BasketStatusDto>(result);
        //var genelList = new List<BankName>(c.BankNames.Include(x => x.BankType)).Distinct().ToList();
        //return new GetAllBasketStatusDtoQueryResponse(res.Adapt<List<BasketStatusDto>>());
       return new GetAllBasketStatusDtoQueryResponse(result);
    }
}