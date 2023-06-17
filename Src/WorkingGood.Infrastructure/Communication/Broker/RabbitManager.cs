using System.Text;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using WorkingGood.Domain.Interfaces.Communication;
using WorkingGood.Log;

namespace WorkingGood.Infrastructure.Communication.Broker;

public class RabbitManager : IRabbitManager
{
    private readonly IWgLog<RabbitManager> _logger;
    private readonly DefaultObjectPool<IModel> _defaultObjectPool;
    public RabbitManager(IWgLog<RabbitManager> logger, IPooledObjectPolicy<IModel> pooledObjectPolicy)
    {
        _logger = logger; 
        _defaultObjectPool = new DefaultObjectPool<IModel>(pooledObjectPolicy);
    }
    public Task Send(string message, string exchangeName, string routingKey)
    {
        _logger.Info($"Sending message: [{message}] on routingKey [{routingKey}] ");
        if (string.IsNullOrEmpty(message))
            return Task.FromCanceled(new CancellationToken());
        var channel = _defaultObjectPool.Get();
        try
        {
            var bytesToSend = Encoding.UTF8.GetBytes(message);
            var properties = channel.CreateBasicProperties();
            properties.ContentType = "application/json";
            channel.BasicPublish(
                exchange: exchangeName,
                routingKey: routingKey,
                basicProperties: properties,
                body: bytesToSend);
        }
        catch (Exception ex)
        {
            _logger.Error(ex);
            throw;
        }
        finally
        {
            _defaultObjectPool.Return(channel);
        }
        return Task.CompletedTask;
    }
}