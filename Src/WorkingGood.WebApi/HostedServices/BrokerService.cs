using System.Text;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WorkingGood.Infrastructure.Common.ConfigModels;
using WorkingGood.Infrastructure.Communication.Broker.Serivces;

namespace WorkingGood.WebApi.HostedServices;

public class BrokerService: BackgroundService
	{
        private readonly RabbitMqConfig _rabbitMqConfig;
        private IBrokerMessageService? _brokerMessageService;
        private IPooledObjectPolicy<IModel>? _defaultObjectPool;
        private IModel? _channel;
		public BrokerService(IServiceScopeFactory scopeFactory, IPooledObjectPolicy<IModel> pooledObjectPolicy, RabbitMqConfig rabbitMqConfig)
		{
            _rabbitMqConfig = rabbitMqConfig;
            _defaultObjectPool = pooledObjectPolicy;
            InitializeServices(scopeFactory);
            InitializeConnection();
		}
        private void InitializeServices(IServiceScopeFactory scopeFactory)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                _brokerMessageService = scope.ServiceProvider.GetRequiredService<IBrokerMessageService>();
                //_defaultObjectPool = scope.ServiceProvider.GetRequiredService<IPooledObjectPolicy<IModel>>();
            }
        }
        private void InitializeConnection()
        {
            _channel =  _defaultObjectPool.Create();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async(ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.Span);
                await HandleMessage(content, ea.RoutingKey);
                _channel!.BasicAck(ea.DeliveryTag, false);
            };
            foreach (var route in _rabbitMqConfig.ReceivingRoutes)
            {
                _channel.BasicConsume(route.RoutingKey, false, consumer);
            }
            return Task.CompletedTask;
        }
        private async Task HandleMessage(string content, string routingKey)
        {
            await _brokerMessageService!.Handle(content, routingKey);
        }
    }