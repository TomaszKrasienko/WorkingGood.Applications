using System;
namespace WorkingGood.Infrastructure.Common.Exceptions
{
	public class BadResponseException : Exception
	{
		public BadResponseException() : base("Message from external service is not incorrect")
		{
		}
	}
}

