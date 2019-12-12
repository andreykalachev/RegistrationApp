using System;
using System.Security.Claims;
using System.Threading.Tasks;
using RegistrationApp.Domain.Core.Identity;

namespace RegistrationApp.Domain.Interfaces.Services.Identity
{
    public interface IUserService
    {
        Task<User> GetById(Guid id);

        Task<ClaimsPrincipal> RegisterAsync(User user);

        Task<ClaimsPrincipal> LoginAsync(User user);

        Task SetUserInRole(User user, Role role);

        Task DeleteUserAsync(Guid userId);
    }
}
