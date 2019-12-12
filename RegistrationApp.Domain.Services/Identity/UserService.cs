using RegistrationApp.Domain.Core.Exceptions;
using RegistrationApp.Domain.Core.Identity;
using RegistrationApp.Domain.Interfaces;
using RegistrationApp.Domain.Interfaces.Repositories.Identity;
using RegistrationApp.Domain.Interfaces.Services.Identity;
using RegistrationApp.Domain.Services.Utilities;
using System;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RegistrationApp.Domain.Services.Identity
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.UserRepository;
        }

        public async Task<User> GetById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new EntityNotFoundException("Cannot find such user in database");
            }

            return user;
        }

        public async Task<ClaimsPrincipal> RegisterAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Unable to register, parameter cannot be null");
            }

            if (await IsLoginUnique(user.Email))
            {
                throw new DomainException("There is already user with such email in database");
            }

            _userRepository.Add(user);
            await _unitOfWork.CommitAsync();

            return ClaimsGenerator.GenerateClaim(user);
        }

        public async Task<ClaimsPrincipal> LoginAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Unable to login, parameter cannot be null");
            }

            if (await IsLoginSuccessful(user))
            {
                throw new AuthenticationException("Unable to login, wrong email or password");
            }

            return ClaimsGenerator.GenerateClaim(user);
        }

        public async Task SetUserInRole(User user, Role role)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            var userToUpdate = await GetById(user.Id);
            userToUpdate.SetRole(role);
            _userRepository.Update(userToUpdate);

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var userToDelete = await _userRepository.GetByIdAsync(userId);

            if (userToDelete == null)
            {
                throw new EntityNotFoundException("Unable to delete, user not found");
            }

            _userRepository.Delete(userToDelete);
            await _unitOfWork.CommitAsync();
        }

        private async Task<bool> IsLoginUnique(string email)
        {
            return await _userRepository.FirstOrDefaultAsync(x => x.Email == email) == null;
        }

        private async Task<bool> IsLoginSuccessful(User user)
        {
            return await _userRepository.FirstOrDefaultAsync(x =>
                x.Email == user.Email && x.Password == user.Password) == null;
        }
    }
}
