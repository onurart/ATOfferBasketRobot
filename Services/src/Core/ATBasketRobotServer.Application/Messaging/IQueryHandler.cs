using MediatR;
namespace ATBasketRobotServer.Application.Messaging;
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{
}