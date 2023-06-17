using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WorkingGood.Domain.Interfaces;
using WorkingGood.Infrastructure.Common.Exceptions;
using WorkingGood.Infrastructure.Communication.Entities;

namespace WorkingGood.Infrastructure.Communication.Broker.Serivces;

public class BrokerMessageService: IBrokerMessageService
{
    private readonly ILogger<BrokerMessageService> _logger;
    private readonly IApplicationRepository _applicationRepository;
    public BrokerMessageService(ILogger<BrokerMessageService> logger, IApplicationRepository applicationRepository)
    {
        _logger = logger;
        _applicationRepository = applicationRepository;
    }
    public async Task Handle(string message, string queueName)
    {
        try
        {
            _logger.LogInformation("Handling message from broker");
            await _applicationRepository.DeleteForOfferAsync(
                GetOfferId(message));
        }
        catch (InvalidBrokerMessage ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private Guid GetOfferId(string message)
    {
        RemoveApplicationMessage removeApplicationMessage =
            JsonConvert.DeserializeObject<RemoveApplicationMessage>(message)
            ?? throw new InvalidBrokerMessage($"Message {message} is invalid");
        return removeApplicationMessage.OfferId;
    }
}