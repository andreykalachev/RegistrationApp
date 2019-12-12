using Microsoft.EntityFrameworkCore;
using RegistrationApp.Domain.Interfaces;
using RegistrationApp.Domain.Interfaces.Repositories;
using RegistrationApp.Domain.Interfaces.Repositories.Identity;
using RegistrationApp.Persistence.Context;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationApp.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(DataContext context,IUserRepository userRepository, IRoleRepository roleRepository, ITodoListRepository todoListRepository)
        {
            _context = context;
            UserRepository = userRepository;
            RoleRepository = roleRepository;
            TodoListRepository = todoListRepository;
        }

        private readonly DataContext _context;

        public IUserRepository UserRepository { get; set; }

        public IRoleRepository RoleRepository { get; set; }

        public ITodoListRepository TodoListRepository { get; set; }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task RejectChangesAsync()
        {
            foreach (var entry in _context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        await entry.ReloadAsync();
                        break;
                }
            }
        }
    }
}
