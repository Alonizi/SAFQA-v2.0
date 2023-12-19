using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Persistence_Layer_Common.DB;
using System.Linq.Expressions;
using System;

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

        public async Task<TEntity> FindAsync(Expression<Func<TEntity,bool>> filter) { 
            return await _dbSet.SingleOrDefaultAsync(filter);
        }

        public async Task<int> SaveChangesAync()
        {
            return await _appDb.SaveChangesAsync();
        }
    }
}