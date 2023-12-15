using MediatR;
namespace ATBasketRobotServer.Application.Messaging;
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}