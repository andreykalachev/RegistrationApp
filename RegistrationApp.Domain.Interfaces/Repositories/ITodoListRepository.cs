using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RegistrationApp.Domain.Core.Entities;

namespace RegistrationApp.Domain.Interfaces.Repositories
{
    public interface ITodoListRepository : IGenericRepository<TodoItem>
    {
        Task<IEnumerable<TodoItem>> GetAllAsync(Expression<Func<TodoItem, bool>> predicate);
    }
}
