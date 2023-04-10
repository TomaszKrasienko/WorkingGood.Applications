using System;
using WorkingGood.Domain.Exceptions;
using WorkingGood.Domain.ValueObjects;

namespace WorkingGood.Domain.Models
{
	public class Application
	{
		public Guid Id { get; private set; }
		public ApplicationCandidate ApplicationCandidate { get; private set; }
		public string Description { get; private set; }
		public byte[] Document { get; private set; }
		public Guid OfferId { get; private set; }
		public Application(
			string cadidateFirstName,
			string cadidateLastName,
			string candidateEmailAddress,
			string description,
			byte[] document)
		{
			Id = Guid.NewGuid();
			ApplicationCandidate = new ApplicationCandidate(
				cadidateFirstName,
				cadidateLastName,
				candidateEmailAddress
				);
			if(!IsDescriptionValid(description))
                throw new DomainException("Description can not be null or empty");
            Description = description;
			Document = document;
		}
		private static bool IsDescriptionValid(string description)
		{
			return !string.IsNullOrEmpty(description);
		}
	}
}

