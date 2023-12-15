using MediatR;
namespace ATBasketRobotServer.Application.Messaging;
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}