using System;
using System.Threading.Tasks;
using RegistrationApp.Domain.Core.Identity;

namespace RegistrationApp.Domain.Interfaces.Services.Identity
{
    public interface IRoleService
    {
        Task<int> AddRoleAsync(Role role);

        Task DeleteRoleAsync(Guid roleId);
    }
}
