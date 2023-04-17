using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using WorkingGood.Infrastructure.Common.ConfigModels;

namespace WorkingGood.Infrastructure.Communication.Broker;

public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
{
    private readonly ILogger<RabbitModelPooledObjectPolicy> _logger;
    private readonly BrokerConfig _brokerConfig;
    private readonly IConnection? _connection;
    public RabbitModelPooledObjectPolicy(ILogger<RabbitModelPooledObjectPolicy> logger, BrokerConfig brokerConfig)
    {
        _logger = logger;
        _brokerConfig = brokerConfig;
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
        connection.HostName = _brokerConfig.Host;
        connection.UserName = _brokerConfig.UserName;
        connection.Password = _brokerConfig.Password;
        if(_brokerConfig.Port != null)
            connection.Port = (int)_brokerConfig.Port;
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