namespace WorkingGood.Infrastructure.Common.ConfigModels;

public class BrokerConfig
{
    public string Host { get; set; } = string.Empty;
    public int? Port { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<RabbitMqRoutesConfig> SendingRoutes { get; set; } = new();
}