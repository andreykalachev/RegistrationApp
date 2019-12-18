using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistrationApp.Domain.Core.Entities;
using RegistrationApp.Domain.Interfaces.Services;
using RegistrationApp.Domain.Interfaces.Services.Identity;
using System;
using System.Threading.Tasks;
using AutoMapper;
using RegistrationApp.Api.Models;

namespace RegistrationApp.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class TodoListController : ControllerBase
    {
        private readonly ITodoListService _todoListService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public TodoListController(ITodoListService todoListService, IUserService userService, IMapper mapper)
        {
            _todoListService = todoListService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("/{controller}/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get(Guid id)
        {
            var item = await _todoListService.GetByIdAsync(id);

            if (!IsAccessPermitted(item))
            {
                return Forbid();
            }

            return Ok(item);
        }

        [HttpGet("/allMyTasks")]
        public async Task<IActionResult> GetAll()
        {
            var userEmail = HttpContext.User.Identity.Name;
            var todoItemsList = await _todoListService.GetAllByUserEmailAsync(userEmail);

            return Ok(todoItemsList);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TodoItemPostViewModel todoItemPostViewModel)
        {
            var item = _mapper.Map<TodoItemPostViewModel, TodoItem>(todoItemPostViewModel);
            var userEmail = HttpContext.User.Identity.Name;
            item.AddedBy = await _userService.GetByEmail(userEmail);
            await _todoListService.AddItemAsync(item);

            return Ok();
        }

        [HttpDelete("/{controller}/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _todoListService.GetByIdAsync(id);

            if (!IsAccessPermitted(item))
            {
                return Forbid();
            }

            await _todoListService.DeleteAsync(id);

            return Ok();
        }

        [HttpPut("/{controller}/markAsDone/{id}")]
        public async Task<IActionResult> MarkAsDone(Guid id, [FromBody]bool isDone = true)
        {
            var item = await _todoListService.GetByIdAsync(id);

            if (!IsAccessPermitted(item))
            {
                return Forbid();
            }

            await _todoListService.MarkAsDoneAsync(item.Id, isDone);

            return Ok(item);
        }

        [HttpPut("/{controller}")]
        public async Task<IActionResult> Put(TodoItemUpdateViewModel updatedItem)
        {
            if (updatedItem == null)
            {
                throw new NullReferenceException("Parameter cannot be null");
            }

            var itemToUpdate = await _todoListService.GetByIdAsync(updatedItem.Id);

            if (!IsAccessPermitted(itemToUpdate))
            {
                return Forbid();
            }

            itemToUpdate.Title = updatedItem.Title;
            itemToUpdate.Description = updatedItem.Description;

            await _todoListService.UpdateAsync(itemToUpdate);

            return Ok();
        }

        private bool IsAccessPermitted(TodoItem item)
        {
            var user = HttpContext.User;

            if (user.IsInRole("Admin"))
            {
                return true;
            }

            var userEmail = user.Identity.Name;
            return item.AddedBy?.Email == userEmail;
        }
    }
}
