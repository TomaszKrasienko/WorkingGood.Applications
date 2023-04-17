using System.Text;
using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using MongoDB.Driver.Core.Operations;
using RabbitMQ.Client;
using WorkingGood.Domain.Interfaces.Communication;

namespace WorkingGood.Infrastructure.Communication.Broker;

public class RabbitManager : IRabbitManager
{
    private readonly ILogger<RabbitManager> _logger;
    private readonly DefaultObjectPool<IModel> _defaultObjectPool;
    public RabbitManager(ILogger<RabbitManager> logger, IPooledObjectPolicy<IModel> pooledObjectPolicy)
    {
        _logger = logger; 
        _defaultObjectPool = new DefaultObjectPool<IModel>(pooledObjectPolicy);
    }
    public Task Send(string message, string exchangeName, string routingKey)
    {
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
            _logger.LogError(ex.Message, ex);
            throw;
        }
        finally
        {
            _defaultObjectPool.Return(channel);
        }
        return Task.CompletedTask;
    }
}