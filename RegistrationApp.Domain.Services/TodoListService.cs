using RegistrationApp.Domain.Core.Entities;
using RegistrationApp.Domain.Core.Exceptions;
using RegistrationApp.Domain.Interfaces;
using RegistrationApp.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RegistrationApp.Domain.Interfaces.Repositories;

namespace RegistrationApp.Domain.Services
{
    public class TodoListService : ITodoListService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITodoListRepository _todoListRepository;

        public TodoListService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _todoListRepository = _unitOfWork.TodoListRepository;
        }
        public async Task<int> AddItemAsync(TodoItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Parameter cannot be null");
            }

            item.DateAdded = DateTime.Now;
            _todoListRepository.Add(item);

            return await _unitOfWork.CommitAsync();
        }

        public async Task MarkAsDoneAsync(TodoItem item)
        {
            var itemToMarkAsDone = await _todoListRepository.GetByIdAsync(item.Id);

            if (itemToMarkAsDone == null)
            {
                throw new EntityNotFoundException("Item not found");
            }

            itemToMarkAsDone.IsDone = true;
            itemToMarkAsDone.DateDone = DateTime.Now;

            await _unitOfWork.CommitAsync();
        }

        public async Task<TodoItem> GetByIdAsync(Guid id)
        {
            return await _todoListRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync(bool includeDoneItems = false)
        {
            if (includeDoneItems == false)
            {
                return await _todoListRepository.GetAllAsync(x => x.IsDone == false);
            }

            return await _todoListRepository.GetAllAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var itemToDelete = await _todoListRepository.GetByIdAsync(id);

            if (itemToDelete == null)
            {
                throw new EntityNotFoundException("Item not found");
            }

            _todoListRepository.Delete(itemToDelete);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(TodoItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Parameter cannot be null");
            }

            _todoListRepository.Update(item);
            await _unitOfWork.CommitAsync();
        }
    }
}
