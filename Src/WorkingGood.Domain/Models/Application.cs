using WorkingGood.Domain.Exceptions;
using WorkingGood.Domain.ValueObjects;

namespace WorkingGood.Domain.Models
{
	public class Application : Entity
	{
		public ApplicationCandidate ApplicationCandidate { get; private set; }
		public string Description { get; private set; }
		public byte[] Document { get; private set; }
		public Guid OfferId { get; private set; }
		public Application(
			string candidateFirstName,
			string candidateLastName,
			string candidateEmailAddress,
			string description,
			byte[] document,
			Guid offerId)
		{
			Id = Guid.NewGuid();
			ApplicationCandidate = new ApplicationCandidate(
				candidateFirstName,
				candidateLastName,
				candidateEmailAddress
				);
			if(!IsDescriptionValid(description))
                throw new DomainException("Description can not be null or empty");
			Description = description;
			Document = document;
			OfferId = offerId;
		}
		private static bool IsDescriptionValid(string description)
		{
			return !string.IsNullOrEmpty(description);
		}
	}
}

