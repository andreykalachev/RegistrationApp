using RegistrationApp.Domain.Core.Identity;
using RegistrationApp.Domain.Interfaces;
using RegistrationApp.Domain.Interfaces.Services.Identity;
using RegistrationApp.Domain.Services.Utilities;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

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
            if (user != null && await IsLoginUnique(user.Email))
            {
                _unitOfWork.UserRepository.Add(user);
                await _unitOfWork.CommitAsync();

                return ClaimsGenerator.GenerateClaim(user);
            }

            return null;
        }

        public async Task<ClaimsPrincipal> LoginAsync(User user)
        {
            if (user != null && await IsLoginSuccessful(user))
            {
                return ClaimsGenerator.GenerateClaim(user);
            }

            return null;
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var userToDelete = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (userToDelete != null)
            {
                _unitOfWork.UserRepository.Delete(userToDelete);
            }

            await _unitOfWork.CommitAsync();
        }

        private async Task<bool> IsLoginUnique(string email)
        {
            return await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == email) == null;
        }

        private async Task<bool> IsLoginSuccessful(User user)
        {
            return await _unitOfWork.UserRepository.FirstOrDefaultAsync(x =>
                x.Login == user.Login && x.Password == user.Password) == null;
        }
    }
}
