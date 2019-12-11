using RegistrationApp.Domain.Interfaces.Repositories;
using RegistrationApp.Domain.Interfaces.Repositories.Identity;
using System.Threading.Tasks;

namespace RegistrationApp.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; set; }

        IRoleRepository RoleRepository { get; set; }

        ITodoListRepository TodoListRepository { get; set; }

        Task<int> CommitAsync();

        Task RejectChangesAsync();
    }
}
