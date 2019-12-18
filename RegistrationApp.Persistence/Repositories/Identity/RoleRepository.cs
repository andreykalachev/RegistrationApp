using RegistrationApp.Domain.Core.Entities.Identity;
using RegistrationApp.Domain.Interfaces.Repositories.Identity;
using RegistrationApp.Persistence.Context;

namespace RegistrationApp.Persistence.Repositories.Identity
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(DataContext context) : base(context)
        {
        }
    }
}
