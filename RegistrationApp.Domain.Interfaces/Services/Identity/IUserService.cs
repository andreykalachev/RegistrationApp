using System;
using System.Threading.Tasks;
using RegistrationApp.Domain.Core.Entities.Identity;
using RegistrationApp.Domain.Core.ValueObjects;

namespace RegistrationApp.Domain.Interfaces.Services.Identity
{
    public interface IUserService
    {
        Task<User> GetById(Guid id);

        Task<User> GetByEmail(string email);

        Task RegisterAsync(User user);

        Task<User> LoginAsync(UserLogin login);

        Task SetUserInRole(User user, Role role);

        Task DeleteUserAsync(Guid userId);
    }
}
