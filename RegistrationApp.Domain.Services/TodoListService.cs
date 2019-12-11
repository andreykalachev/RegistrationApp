using RegistrationApp.Domain.Core.Entities;
using RegistrationApp.Domain.Interfaces;
using RegistrationApp.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RegistrationApp.Domain.Services
{
    public class TodoListService : ITodoListService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TodoListService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> AddItemAsync(TodoItem item)
        {
            item.DateAdded = DateTime.Now;
            _unitOfWork.TodoListRepository.Add(item);

            return await _unitOfWork.CommitAsync();
        }

        public async Task MarkAsDoneAsync(TodoItem item)
        {
            var itemToMarkAsDone = await _unitOfWork.TodoListRepository.GetByIdAsync(item.Id);

            if (itemToMarkAsDone != null)
            {
                itemToMarkAsDone.IsDone = true;
                itemToMarkAsDone.DateDone = DateTime.Now;

                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<TodoItem> GetByIdAsync(Guid id)
        {
            return await _unitOfWork.TodoListRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync(bool includeDoneItems = false)
        {
            if (includeDoneItems == false)
            {
                return await _unitOfWork.TodoListRepository.GetAllAsync(x => x.IsDone == false);
            }

            return await _unitOfWork.TodoListRepository.GetAllAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var itemToDelete = await _unitOfWork.TodoListRepository.GetByIdAsync(id);

            if (itemToDelete != null)
            {
                _unitOfWork.TodoListRepository.Delete(itemToDelete);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(TodoItem item)
        {
            _unitOfWork.TodoListRepository.Update(item);

            await _unitOfWork.CommitAsync();
        }
    }
}
