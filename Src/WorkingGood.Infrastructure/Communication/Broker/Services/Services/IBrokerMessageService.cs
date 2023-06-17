namespace WorkingGood.Infrastructure.Communication.Broker.Serivces;

public interface IBrokerMessageService
{
    Task Handle(string message, string queueName);
}