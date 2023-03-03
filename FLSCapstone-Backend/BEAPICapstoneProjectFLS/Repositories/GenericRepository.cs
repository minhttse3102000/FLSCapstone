using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace BEAPICapstoneProjectFLS.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private FLSCapstoneDatabaseContext _context;
        private DbSet<T> _dbSet = null;


        public GenericRepository(FLSCapstoneDatabaseContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public IQueryable<T> GetAllByIQueryable()
        {
            return _dbSet;
        }

        public T GetByID(string id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> GetByIDAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Insert(T obj)
        {
            _dbSet.Add(obj);
        }

        public async Task InsertAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
        }

        public void Update(T obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public async Task UpdateAsync(T obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public void Delete(string id)
        {
            T existing = _dbSet.Find(id);
            _dbSet.Remove(existing);
        }

        public async Task DeleteAsync(string id)
        {
            T existing = await _dbSet.FindAsync(id);
            _dbSet.Remove(existing);
            await _context.SaveChangesAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public T Find(Expression<Func<T, bool>> match)
        {
            return _dbSet.FirstOrDefault(match);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _dbSet.FirstOrDefaultAsync(match);
        }

        public IQueryable<T> sortAsc<TResult>(Expression<Func<T, TResult>> match, IQueryable<T> list)
        {
            return list.Select(x => x).OrderBy(match);
        }

        public async Task DeleteSpecificFieldByAsync(Expression<Func<T, bool>> prematch)
        {
            var rowDeleted = await FindByAsync(prematch);
            _dbSet.RemoveRange(rowDeleted);
        }
    }
}
