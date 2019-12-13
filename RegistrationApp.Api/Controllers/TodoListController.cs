using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RegistrationApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoListController : ControllerBase
    {
        private static readonly string[] MyTodoList = new[]
        {
            "Do homework", "Buy food", "Make a sandwich", "Call mom"
        };

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult Get()
        {
            return Ok(MyTodoList);
        }
    }
}
