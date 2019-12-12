using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RegistrationApp.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(Guid id);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
