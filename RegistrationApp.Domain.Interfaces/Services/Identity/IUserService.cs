using System;
using System.Security.Claims;
using System.Threading.Tasks;
using RegistrationApp.Domain.Core.Identity;

namespace RegistrationApp.Domain.Interfaces.Services.Identity
{
    public interface IUserService
    {
        Task<ClaimsPrincipal> RegisterAsync(User user);

        Task<ClaimsPrincipal> LoginAsync(User user);

        Task DeleteUserAsync(Guid userId);
    }
}
