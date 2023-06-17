using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using WorkingGood.Infrastructure.Common.ConfigModels;
using WorkingGood.Log;

namespace WorkingGood.Infrastructure.Communication.Broker;

public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
{
    private readonly IWgLog<RabbitModelPooledObjectPolicy> _logger;
    private readonly RabbitMqConfig _rabbitMqConfig;
    private readonly IConnection? _connection;
    public RabbitModelPooledObjectPolicy(IWgLog<RabbitModelPooledObjectPolicy> logger, RabbitMqConfig rabbitMqConfig)
    {
        _logger = logger;
        _rabbitMqConfig = rabbitMqConfig;
        try
        {
            _connection = GetConnection();
        }
        catch (Exception ex)
        {
            _logger.Error(ex);
            throw;
        }
    }

    private IConnection GetConnection()
    {
        try
        {
            ConnectionFactory connection = new ConnectionFactory();
            connection.HostName = _rabbitMqConfig.Host;
            connection.UserName = _rabbitMqConfig.UserName;
            connection.Password = _rabbitMqConfig.Password;
            if (_rabbitMqConfig.Port != null)
                connection.Port = (int) _rabbitMqConfig.Port;
            return connection.CreateConnection();
        }
        catch (Exception ex)
        {
            _logger.Error(ex);
            throw;
        }
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