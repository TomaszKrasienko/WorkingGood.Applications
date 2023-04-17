namespace WorkingGood.Domain.Interfaces.Communication;

public interface IRabbitManager
{
    public Task Send(string message, string exchangeName, string routingKey);
}