using System;
using WorkingGood.Domain.Models;

namespace WorkingGood.Domain.Interfaces
{
	public interface IApplicationRepository : IRepository<Application>
	{
		Task DeleteForOfferAsync(Guid offerId);
	}
}

