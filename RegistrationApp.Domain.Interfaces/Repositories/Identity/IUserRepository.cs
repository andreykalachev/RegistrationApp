using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RegistrationApp.Domain.Core.Identity;

namespace RegistrationApp.Domain.Interfaces.Repositories.Identity
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate);
    }
}
