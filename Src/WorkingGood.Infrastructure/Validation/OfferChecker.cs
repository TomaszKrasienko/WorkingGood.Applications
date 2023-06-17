using System;
using Microsoft.Extensions.Logging;
using WorkingGood.Domain.Interfaces.Valida;
using WorkingGood.Infrastructure.Common.Extensions;
using WorkingGood.Infrastructure.Common.Statics;
using WorkingGood.Infrastructure.Communication.Entities;
using WorkingGood.Log;

namespace WorkingGood.Infrastructure.Validation
{
	public class OfferChecker : IOfferChecker
	{
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWgLog<OfferChecker> _logger;
		public OfferChecker(IWgLog<OfferChecker> logger, IHttpClientFactory httpClientFactory)
		{
			_logger = logger;
			_httpClientFactory = httpClientFactory;
		}
		public async Task<bool> CheckOfferStatus(Guid offerId)
		{
			try
			{
				var httpClient = _httpClientFactory.CreateClient(HttpClients.OffersClient);
				var response = await httpClient.GetAsync($"offers/getOfferStatus/{offerId}");
				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					return (bool?) content.DeserializeExternalMessage<BaseMessage>().Object ?? false;
				}
				else
				{
					return false;
				}
			}
			catch (HttpRequestException ex)
			{
				_logger.Error(ex);
				return false;
			}
		}
    }
}

