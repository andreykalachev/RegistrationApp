using System;
using System.Threading.Tasks;

namespace RegistrationApp.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(Guid id);

        void Add(TEntity entity);

        void Delete(TEntity entity);
    }
}
