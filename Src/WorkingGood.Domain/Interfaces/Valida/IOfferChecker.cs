using System;
namespace WorkingGood.Domain.Interfaces.Valida
{
	public interface IOfferChecker
	{
		Task<bool> CheckOfferStatus(Guid offerId);
	}
}

