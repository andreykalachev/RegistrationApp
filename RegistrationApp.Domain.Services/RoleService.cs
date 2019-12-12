using RegistrationApp.Domain.Core.Exceptions;
using RegistrationApp.Domain.Core.Identity;
using RegistrationApp.Domain.Interfaces;
using RegistrationApp.Domain.Interfaces.Services.Identity;
using System;
using System.Threading.Tasks;

namespace RegistrationApp.Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddRoleAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role), "Unable to add role, parameter cannot be null");
            }

            _unitOfWork.RoleRepository.Add(role);
            return await _unitOfWork.CommitAsync();
        }

        public async Task DeleteRoleAsync(Guid roleId)
        {
            var roleToDelete = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);

            if (roleToDelete == null)
            {
                throw new EntityNotFoundException("Unable to delete, role not found");
            }

            _unitOfWork.RoleRepository.Delete(roleToDelete);
            await _unitOfWork.CommitAsync();
        }
    }
}
