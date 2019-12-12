using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RegistrationApp.Domain.Core.Entities;

namespace RegistrationApp.Domain.Interfaces.Repositories
{
    public interface ITodoListRepository : IGenericRepository<TodoItem>
    {
        void Update(TodoItem entity);

        Task<IEnumerable<TodoItem>> GetAllAsync();

        Task<IEnumerable<TodoItem>> GetAllAsync(Expression<Func<TodoItem, bool>> predicate);
    }
}
