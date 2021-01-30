using ModelChecker.DAL.EF;
using ModelChecker.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ModelChecker.DAL.Repositories
{
	public class Repository<T> : IRepository<T> where T : class
	{
		protected readonly ModelCheckerContext db;
		protected DbSet<T> dbSet;
		public Repository(ModelCheckerContext context)
		{
			db = context;
			dbSet = context.Set<T>();
		}

		public virtual async Task CreateNotDetectAsync(T item)
		{
			db.Configuration.AutoDetectChangesEnabled = false;
			dbSet.Add(item);
			await db.SaveChangesAsync();
			db.Configuration.AutoDetectChangesEnabled = true;
		}

		public virtual async Task CreateAsync(T item)
		{
			dbSet.Add(item);
			await db.SaveChangesAsync();
		}

		public virtual async Task CreateAsync(IEnumerable<T> items)
		{
			dbSet.AddRange(items);
			await db.SaveChangesAsync();
		}

		public virtual async Task DeleteAsync(int id)
		{
			T item = await dbSet.FindAsync(id);
			if (item != null)
				dbSet.Remove(item);
			await db.SaveChangesAsync();
		}

		public virtual async Task DeleteAsync(T item)
		{
			dbSet.Remove(item);
			await db.SaveChangesAsync();
		}

		public virtual IEnumerable<T> Find(Func<T, bool> predicate)
		{
			return dbSet.AsNoTracking().Where(predicate).ToList();
		}

		public virtual async Task<IEnumerable<T>> GetAllAsync()
		{
			return await dbSet.ToListAsync();
		}

		public virtual async Task<T> GetAsync(int id)
		{
			return await dbSet.FindAsync(id);
		}

		public virtual T Get(int Id)
		{
			return dbSet.Find(Id);
		}
		public virtual async Task UpdateNotDetectAsync(T item)
		{
			db.Configuration.AutoDetectChangesEnabled = false;
			db.Entry(item).State = EntityState.Modified;
			await db.SaveChangesAsync();
			db.Configuration.AutoDetectChangesEnabled = true;
		}

		public virtual async Task UpdateAsync(T item)
		{
			db.Entry(item).State = EntityState.Modified;
			await db.SaveChangesAsync();
		}

		public virtual async Task<IEnumerable<T>> GetWithIncludeAsync(params Expression<Func<T, object>>[] includeProperties)
		{
			return await Include(includeProperties).ToListAsync();
		}

		public virtual IEnumerable<T> GetWithInclude(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> query = Include(includeProperties);
			return query.Where(predicate).ToList();
		}

		public virtual IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> query = dbSet.AsNoTracking();
			return includeProperties
				.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
		}

		//public virtual void ExecuteQuery(string sql)
		//{
		//	db.Database.ExecuteSqlCommand(sql);
		//}

		//public virtual T Get(string Key)
		//{
		//	return dbSet.Find(Key);
		//}

	

		public virtual IQueryable<T> GetAll()
		{
			return dbSet.AsQueryable();
		}

	}
}

