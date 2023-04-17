using System;
using System.Linq.Expressions;
using WorkingGood.Domain.Models;

namespace WorkingGood.Domain.Interfaces
{
	public interface IRepository<T> where T : Entity
	{
		Task<T> AddAsync(T entity);
        Task<T> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> includeProperties);
	}
}

