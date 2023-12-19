using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Persistence_Layer_Common.Repository{


    public interface IGenericRepository<TEntity> where TEntity: class{

        public Task<TEntity> FindAsync(Expression<Func<TEntity,bool>> filter);

        public Task<int> AddAsync(TEntity entity);

        public Task<int> SaveChangesAync();

        
    }
}