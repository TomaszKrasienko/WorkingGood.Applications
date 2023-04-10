using System;
namespace WorkingGood.Domain.Interfaces
{
	public interface IRepository<T>
	{
		Task<T> AddAsync(T entity);
	}
}

