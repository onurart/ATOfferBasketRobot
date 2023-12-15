using ATBasketRobotServer.Domain.Dtos;
namespace ATBasketRobotServer.Application.Services;
public interface IRabbitMQService
{
    void SendQueue(ReportDto reportDto);
}