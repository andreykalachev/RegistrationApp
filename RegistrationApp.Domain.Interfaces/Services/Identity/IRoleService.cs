using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RegistrationApp.Domain.Core.Entities.Identity;

namespace RegistrationApp.Domain.Interfaces.Services.Identity
{
    public interface IRoleService
    {
        Task<Role> GetById(Guid id);

        Task<Role> GetByName(string name);

        Task<IEnumerable<Role>> GetAllRolesAsync();

        Task<int> AddRoleAsync(Role role);

        Task DeleteRoleAsync(Guid roleId);
    }
}
