using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Persistence_Layer_Common.DB;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Persistence_Layer_Common.Repository{


    public class GenericRepository<TEntity> :IGenericRepository<TEntity> where TEntity: class{
        private readonly ApplicationDbContext _appDb;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext appDb){
            _appDb = appDb;
            _dbSet = _appDb.Set<TEntity>();
        }

        public Task<int> AddAsync(TEntity entity)
        {
              _dbSet.AddAsync(entity);
              return _appDb.SaveChangesAsync();
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _dbSet.Where(filter);

            foreach (var include in includes) {
               query= query.Include(include);
            }

            return await query.SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _dbSet.Where(filter);

            foreach (var include in includes) {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<int> SaveChangesAync() => await _appDb.SaveChangesAsync();        

    }
}