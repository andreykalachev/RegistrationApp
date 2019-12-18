using RegistrationApp.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RegistrationApp.Domain.Interfaces.Services
{
    public interface ITodoListService
    {
        Task<int> AddItemAsync(TodoItem item);

        Task MarkAsDoneAsync(Guid itemId, bool isDone = true);

        Task<TodoItem> GetByIdAsync(Guid id);

        Task<IEnumerable<TodoItem>> GetAllAsync(bool includeDoneItems = false);

        Task<IEnumerable<TodoItem>> GetAllByUserEmailAsync(string userEmail);

        Task DeleteAsync(Guid id);

        Task UpdateAsync(TodoItem item);
    }
}
