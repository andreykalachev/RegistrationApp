using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RegistrationApp.Domain.Core.Identity;
using RegistrationApp.Domain.Interfaces.Services.Identity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AccountController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [HttpPost("/role")]
        public async Task<IActionResult> Post([FromBody] Role role)
        {
            await _roleService.AddRoleAsync(role);

            return Ok();
        }

        [HttpGet("/role")]
        public async Task<IActionResult> Get(Guid id)
        {
            var role = await _roleService.GetById(id);

            return Ok(role);
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Post(User user)
        {
            var identity = await _userService.RegisterAsync(user);

            var secretString = "MySecretString";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretString);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(token);
        }
    }
}