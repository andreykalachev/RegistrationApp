using RegistrationApp.Domain.Core.Exceptions;
using RegistrationApp.Domain.Core.Identity;
using RegistrationApp.Domain.Interfaces;
using RegistrationApp.Domain.Interfaces.Repositories.Identity;
using RegistrationApp.Domain.Interfaces.Services.Identity;
using System;
using System.Threading.Tasks;

namespace RegistrationApp.Domain.Services.Identity
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _roleRepository = _unitOfWork.RoleRepository;
        }

        public async Task<int> AddRoleAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role), "Unable to add role, parameter cannot be null");
            }

            if (await IsNameUnique(role) == false)
            {
                throw new DomainException("Role with such name already exists");
            }

            _roleRepository.Add(role);
            return await _unitOfWork.CommitAsync();
        }

        public async Task DeleteRoleAsync(Guid roleId)
        {
            var roleToDelete = await _roleRepository.GetByIdAsync(roleId);

            if (roleToDelete == null)
            {
                throw new EntityNotFoundException("Unable to delete, role not found");
            }

            _roleRepository.Delete(roleToDelete);
            await _unitOfWork.CommitAsync();
        }

        private async Task<bool> IsNameUnique(Role role)
        {
            return await _roleRepository.FirstOrDefaultAsync(x => x.Name == role.Name) == null;
        }
    }
}
