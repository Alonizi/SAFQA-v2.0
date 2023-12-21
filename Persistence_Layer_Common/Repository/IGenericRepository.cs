using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence_Layer_Common.Repository{


    public interface IGenericRepository<TEntity> where TEntity: class{

       // public Task<TEntity> FindAsync(Expression<Func<TEntity,bool>> filter);
        
        public Task<TEntity> FindAsync (Expression<Func<TEntity,bool>> filter , params Expression<Func<TEntity,Object>>[] includes) ;

        public Task<int> AddAsync(TEntity entity);
        
        public Task<int> SaveChangesAync();

        public Task<IEnumerable<TEntity>> FindAllAsync (Expression<Func<TEntity,bool>> filter , params Expression<Func<TEntity,Object>>[] includes) ;

        
    }
}