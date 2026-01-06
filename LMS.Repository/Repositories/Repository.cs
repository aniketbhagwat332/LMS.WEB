using LMS.Repository.Data;
using LMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly LMSDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(LMSDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Delete(T entity)
            => _dbSet.Remove(entity);

        public async Task SaveAsync()
            => await _context.SaveChangesAsync();

        public Task<int> CountAsync()
        {
            return  _context.Set<T>().CountAsync();
        }

        public async Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>()
                                 .Where(predicate)
                                 .ToListAsync();
        }

    }
}
