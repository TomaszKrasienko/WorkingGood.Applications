namespace WorkingGood.Infrastructure.Common.ConfigModels;

public record RabbitMqConfig
{
    public string Host { get; init; } = string.Empty;
    public int? Port { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public List<RabbitMqRoutesConfig> SendingRoutes { get; init; } = new();
    public List<RabbitMqRoutesConfig> ReceivingRoutes { get; init; } = new();
}