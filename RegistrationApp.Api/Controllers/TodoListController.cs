using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistrationApp.Domain.Core.Entities;
using RegistrationApp.Domain.Interfaces.Services;
using RegistrationApp.Domain.Interfaces.Services.Identity;
using System;
using System.Threading.Tasks;

namespace RegistrationApp.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class TodoListController : ControllerBase
    {
        private readonly ITodoListService _todoListService;
        private readonly IUserService _userService;

        public TodoListController(ITodoListService todoListService, IUserService userService)
        {
            _todoListService = todoListService;
            _userService = userService;
        }

        public IActionResult GetStatic()
        {
            return Ok(new[] { "Do homework", "Buy food", "Make a sandwich", "Call mom" });
        }

        [HttpGet]
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

        [HttpPost]
        public async Task<IActionResult> Post(TodoItem item)
        {
            var userEmail = HttpContext.User.Identity.Name;
            item.AddedBy = await _userService.GetByEmail(userEmail);
            await _todoListService.AddItemAsync(item);

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
