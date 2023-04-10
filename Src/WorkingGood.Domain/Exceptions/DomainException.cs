using System;
namespace WorkingGood.Domain.Exceptions
{
	public class DomainException : Exception
	{
		public DomainException(string msg) : base(msg)
		{

		}
	}
}

