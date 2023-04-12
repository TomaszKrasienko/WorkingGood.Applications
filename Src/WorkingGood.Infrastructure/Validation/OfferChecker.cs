using System;
using WorkingGood.Domain.Interfaces.Valida;
using WorkingGood.Infrastructure.Common.Statics;

namespace WorkingGood.Infrastructure.Validation
{
	public class OfferChecker : IOfferChecker
	{
        private readonly IHttpClientFactory _httpClientFactory;
		public OfferChecker(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}
		public async Task<bool> CheckOfferStatus(Guid offerId)
		{
			//Utworzenie klienta API
			var httpClient = _httpClientFactory.CreateClient(HttpClients.OffersClient);
			//Wywołanie API
			//
			var response = await httpClient.GetAsync($"Offers/CheckOfferStatus/{offerId}");
			//Odczytanie JSON z odpowiedzi
			var content = await response.Content.ReadAsStringAsync();
			return true;
		}
    }
}

