using ATBasketRobotServer.Application.Features.CompanyFeatures.LogFeatures.Queires.GetLogsByTableName;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Dtos;
using EntityFrameworkCorePagination.Nuget.Pagination;
namespace ATBasketRobotServer.Application.Services.CompanyServices;
public interface ILogService
{
    Task AddAsync(Log log, string companyId);
    Task<PaginationResult<LogDto>> GetAllByTableName(GetLogsByTableNameQuery request);
}