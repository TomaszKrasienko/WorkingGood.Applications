namespace WorkingGood.Infrastructure.Common.ConfigModels;

public class RabbitMqRoutesConfig
{
    public string Destination { get; set; } = string.Empty;
    public string Exchange { get; set; } = string.Empty;
    public string RoutingKey { get; set; } = string.Empty;
}