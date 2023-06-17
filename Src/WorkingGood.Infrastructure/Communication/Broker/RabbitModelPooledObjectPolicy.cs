using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using WorkingGood.Infrastructure.Common.ConfigModels;

namespace WorkingGood.Infrastructure.Communication.Broker;

public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
{
    private readonly ILogger<RabbitModelPooledObjectPolicy> _logger;
    private readonly RabbitMqConfig _rabbitMqConfig;
    private readonly IConnection? _connection;
    public RabbitModelPooledObjectPolicy(ILogger<RabbitModelPooledObjectPolicy> logger, RabbitMqConfig rabbitMqConfig)
    {
        _logger = logger;
        _rabbitMqConfig = rabbitMqConfig;
        try
        {
            _connection = GetConnection();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            throw;
        }
    }

    private IConnection GetConnection()
    {
        ConnectionFactory connection = new ConnectionFactory();
        connection.HostName = _rabbitMqConfig.Host;
        connection.UserName = _rabbitMqConfig.UserName;
        connection.Password = _rabbitMqConfig.Password;
        if(_rabbitMqConfig.Port != null)
            connection.Port = (int)_rabbitMqConfig.Port;
        return connection.CreateConnection();
    }
    public IModel Create()
    {
        return _connection?.CreateModel()!;
    }

    public bool Return(IModel obj)
    {
        if (obj.IsOpen)
        {
            return true;
        }
        else
        {
            obj?.Dispose();
            return false;
        }
    }
}