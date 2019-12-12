using RegistrationApp.Domain.Core.Identity;
using RegistrationApp.Domain.Interfaces;
using RegistrationApp.Domain.Interfaces.Services.Identity;
using RegistrationApp.Domain.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using RegistrationApp.Domain.Core.Exceptions;

namespace RegistrationApp.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ClaimsPrincipal> RegisterAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "unable to register, parameter cannot be null");
            }

            if (await IsLoginUnique(user.Email))
            {
                throw new LoginException("There is already user with such email in database");
            }

            _unitOfWork.UserRepository.Add(user);
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
                throw new LoginException("Unable to login, wrong email or password");
            }

            return ClaimsGenerator.GenerateClaim(user);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var userToDelete = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (userToDelete == null)
            {
                throw new KeyNotFoundException("Unable to delete, user not found");
            }

            _unitOfWork.UserRepository.Delete(userToDelete);
            await _unitOfWork.CommitAsync();
        }

        private async Task<bool> IsLoginUnique(string email)
        {
            return await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == email) == null;
        }

        private async Task<bool> IsLoginSuccessful(User user)
        {
            return await _unitOfWork.UserRepository.FirstOrDefaultAsync(x =>
                x.Email == user.Email && x.Password == user.Password) == null;
        }
    }
}
