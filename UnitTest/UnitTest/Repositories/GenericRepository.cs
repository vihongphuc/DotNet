using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnitTest.Models;

namespace UnitTest.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        Task<T> FindById(int id);
        Task<IEnumerable<T>> FindAll();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);

        Task<bool> Add(T entity);
        Task<bool> Add(T entity, bool notSave = true);
        Task<bool> Update(T entity);
        Task<bool> Update(T entity, bool notSave = true);

        Task<bool> Remove(T entity);
        Task<bool> Remove(T entity, bool notSave = true);
        Task<bool> Remove(int id);
        Task<bool> Remove(int id, bool notSave = true);

        Task<bool> AddRange(IEnumerable<T> entities);
        Task<bool> AddRange(IEnumerable<T> entities, bool notSave = true);
        Task<bool> RemoveRange(IEnumerable<T> entities);
        Task<bool> RemoveRange(IEnumerable<T> entities, bool notSave = true);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AppDbContext _context;
        protected DbSet<T> _dbSet;
        protected readonly ILogger _logger;


        public GenericRepository(AppDbContext context,
                                ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _logger = logger;

            this._dbSet = context.Set<T>();
        }

        #region Transactions

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        #endregion

        #region Queries

        public virtual async Task<T> FindById(int id)
        {
            return (await _dbSet.FindAsync(id));
        }
        public virtual async Task<IEnumerable<T>> FindAll()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToArrayAsync();
        }

        #endregion

        #region Add & Update & Delete
        public virtual async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            var succeed = (await _context.SaveChangesAsync()) > 0;
            return succeed;
        }
        public virtual async Task<bool> Add(T entity, bool notSave = true)
        {
            var succeed = true;
            if (notSave)
            {
                await AddEntity(entity);
            }
            else
            {
                succeed = await Add(entity);
            }

            return succeed;
        }

        public virtual async Task<bool> Update(T entity)
        {
            UpdateEntityState(entity);
            var succeed = (await _context.SaveChangesAsync()) > 0;
            return succeed;
        }
        public virtual async Task<bool> Update(T entity, bool notSave = true)
        {
            var succeed = true;
            if (notSave)
            {
                UpdateEntityState(entity);
            }
            else
            {
                succeed = await Update(entity);
            }

            return succeed;
        }
        public virtual async Task<bool> Remove(T entity)
        {
            _dbSet.Remove(entity);
            var succeed = (await _context.SaveChangesAsync()) > 0;
            return succeed;
        }
        public virtual async Task<bool> Remove(T entity, bool notSave = true)
        {
            var succeed = true;
            if (notSave)
            {
                RemoveEntity(entity);
            }
            else
            {
                succeed = await Remove(entity);
            }

            return succeed;
        }

        public virtual async Task<bool> Remove(int id)
        {
            var entity = await FindById(id);
            return await Remove(entity);
        }

        public virtual async Task<bool> Remove(int id, bool notSave = true)
        {
            var succeed = true;
            var entity = await FindById(id);
            if (notSave)
            {
                RemoveEntity(entity);
            }
            else
            {
                succeed = await Remove(entity);
            }

            return succeed;
        }
        #endregion

        #region Add & Delete Ranges

        public async Task<bool> AddRange(IEnumerable<T> entities)
        {
            await AddEntityRange(entities);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public virtual async Task<bool> AddRange(IEnumerable<T> entities, bool notSave = true)
        {
            var succeed = true;
            if (notSave)
            {
                await AddEntityRange(entities);
            }
            else
            {
                succeed = await AddRange(entities);
            }

            return succeed;
        }

        public async Task<bool> RemoveRange(IEnumerable<T> entities)
        {
            RemoveEntityRange(entities);

            return (await _context.SaveChangesAsync()) > 0;
        }
        public virtual async Task<bool> RemoveRange(IEnumerable<T> entities, bool notSave = true)
        {
            var succeed = true;
            if (notSave)
            {
                RemoveEntityRange(entities);
            }
            else
            {
                succeed = await RemoveRange(entities);
            }

            return succeed;
        }
        #endregion

        #region privates

        private async Task AddEntity(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        private async Task AddEntityRange(IEnumerable<T> entities)
        {
            //foreach (var entity in entities)
            //    _dbSet.Add(entity);

            await _dbSet.AddRangeAsync(entities);
        }
        private void RemoveEntity(T entity)
        {
            _dbSet.Remove(entity);
        }
        private void RemoveEntityRange(IEnumerable<T> entities)
        {
            //foreach (var entity in entities)
            //    _dbSet.Remove(entity);

            _dbSet.RemoveRange(entities);
        }
        private void UpdateEntityState(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        #endregion
    }
}
