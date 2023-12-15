namespace ATBasketRobotServer.Domain.Abstractions;

public interface IEntity<T>
{
    T Id { get; set; }
}
