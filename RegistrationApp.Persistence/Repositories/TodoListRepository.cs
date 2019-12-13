using Microsoft.EntityFrameworkCore;
using RegistrationApp.Domain.Core.Entities;
using RegistrationApp.Domain.Interfaces.Repositories;
using RegistrationApp.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RegistrationApp.Persistence.Repositories
{
    public class TodoListRepository : GenericRepository<TodoItem>, ITodoListRepository
    {
        public TodoListRepository(DataContext context) : base(context)
        {
        }

        public override async Task<TodoItem> GetByIdAsync(Guid id)
        {
            return await FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<TodoItem> FirstOrDefaultAsync(Expression<Func<TodoItem, bool>> predicate)
        {
            return await DbSet.Include(x => x.AddedBy).FirstOrDefaultAsync(predicate);
        }

        public override async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await DbSet.Include(x => x.AddedBy).ToListAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync(Expression<Func<TodoItem, bool>> predicate)
        {
            return await DbSet.Include(x => x.AddedBy).Where(predicate).ToListAsync();
        }
    }
}
