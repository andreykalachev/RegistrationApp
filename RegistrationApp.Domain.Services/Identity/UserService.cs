using RegistrationApp.Domain.Core.Entities.Identity;
using RegistrationApp.Domain.Core.Exceptions;
using RegistrationApp.Domain.Core.ValueObjects;
using RegistrationApp.Domain.Interfaces;
using RegistrationApp.Domain.Interfaces.Repositories.Identity;
using RegistrationApp.Domain.Interfaces.Services.Identity;
using System;
using System.Security.Authentication;
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
                throw new EntityNotFoundException("User not found");
            }

            return user;
        }

        public async Task RegisterAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Parameter cannot be null");
            }

            if (!await IsLoginUnique(user.Email))
            {
                throw new DomainException("User with such email already exists");
            }

            _userRepository.Add(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task<User> LoginAsync(UserLogin login)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login), "Parameter cannot be null");
            }

            var loggedInUser = await LoginIntoUser(login);

            if (loggedInUser == null)
            {
                throw new AuthenticationException("Wrong email or password");
            }

            return loggedInUser;
        }

        public async Task SetUserInRole(User user, Role role)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            var userToUpdate = await GetById(user.Id);
            userToUpdate.Role = role;
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

        private async Task<User> LoginIntoUser(UserLogin login)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x =>
                x.Email == login.Email && x.Password == login.Password);

            return user;
        }
    }
}
