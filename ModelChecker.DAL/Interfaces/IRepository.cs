using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModelChecker.DAL.Interfaces
{
	public interface IRepository<T>
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<T> GetAsync(int id);
		//T Get(string Key);
		T Get(int Id);
		Task CreateNotDetectAsync(T item);
		Task CreateAsync(T item);
		Task CreateAsync(IEnumerable<T> items);
		Task UpdateNotDetectAsync(T item);
		Task UpdateAsync(T item);
		Task DeleteAsync(int id);
		Task DeleteAsync(T item);
		Task<IEnumerable<T>> GetWithIncludeAsync(params Expression<Func<T, object>>[] includeProperties);
		IEnumerable<T> GetWithInclude(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties);
		IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties);
		IQueryable<T> GetAll();
		//void ExecuteQuery(string sql);

	}
}
