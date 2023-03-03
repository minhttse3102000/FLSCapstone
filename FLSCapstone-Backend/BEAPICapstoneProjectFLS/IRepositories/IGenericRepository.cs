using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        public IQueryable<T> GetAllByIQueryable();
        T GetByID(string id);
        Task<T> GetByIDAsync(string id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        void Insert(T obj);
        Task InsertAsync(T obj);
        void Update(T obj);
        Task UpdateAsync(T obj);
        void Delete(string id);
        Task DeleteAsync(string id);
        void Save();
        Task SaveAsync();
        T Find(Expression<Func<T, bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        IQueryable<T> sortAsc<TResult>(Expression<Func<T, TResult>> match, IQueryable<T> list);

        Task DeleteSpecificFieldByAsync(Expression<Func<T, bool>> prematch);
    }
}
