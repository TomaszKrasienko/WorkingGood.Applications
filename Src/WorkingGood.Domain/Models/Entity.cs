using System;
namespace WorkingGood.Domain.Models
{
	public abstract class Entity
	{
		public Guid Id { get; protected set; }
	}
}

