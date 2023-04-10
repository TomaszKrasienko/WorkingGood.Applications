using System;
using System.Text.RegularExpressions;
using WorkingGood.Domain.Exceptions;

namespace WorkingGood.Domain.ValueObjects
{
	public class ApplicationCandidate
	{
		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public string EmailAddress { get; private set; }
		public ApplicationCandidate(string firstName, string lastName, string emailAddress)
		{
			if (!IsNameValid(firstName))
				throw new DomainException("FirstName must have at least 4 signs");
			FirstName = firstName;
            if (!IsNameValid(lastName))
                throw new DomainException("LastName must have at least 4 signs");
            LastName = lastName;
			if(!IsEmailValid(emailAddress))
                throw new DomainException("Email is incorrect");
            EmailAddress = emailAddress;
		}
		private static bool IsNameValid(string name)
		{
			return !(name is null || name.Length <= 0);
        }
		private static bool IsEmailValid(string email)
		{
			//string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov|pl)$";
			//return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
			return true;
        }
	}
}

