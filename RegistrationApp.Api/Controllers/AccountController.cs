using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RegistrationApp.Api.Models;
using RegistrationApp.Api.Utilities.Authentication;
using RegistrationApp.Domain.Core.Entities.Identity;
using RegistrationApp.Domain.Core.ValueObjects;
using RegistrationApp.Domain.Interfaces.Services.Identity;
using System;
using System.Threading.Tasks;

namespace RegistrationApp.Api.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly JwtTokenGeneratorForAuthentication _jwtTokenGeneratorForAuthentication;

        public AccountController(IUserService userService, IRoleService roleService, JwtTokenGeneratorForAuthentication jwtTokenGeneratorForAuthentication, IMapper mapper)
        {
            _userService = userService;
            _roleService = roleService;
            _jwtTokenGeneratorForAuthentication = jwtTokenGeneratorForAuthentication;
            _mapper = mapper;
        }

        [HttpPost("/role")]
        public async Task<IActionResult> Post(Role role)
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

        [HttpGet("/register")]
        public async Task<IActionResult> Register()
        {
            var listOfRoles = await _roleService.GetAllRolesAsync();

            return Ok(listOfRoles);
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register(UserRegistrationData registrationData)
        {
            var user = _mapper.Map<UserRegistrationData, User>(registrationData);
            await _userService.RegisterAsync(user);
            var jwtAuthenticationToken = _jwtTokenGeneratorForAuthentication.GenerateTokenForUser(user);

            return Ok(jwtAuthenticationToken);
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(UserLogin login)
        {
            var loggedInUser = await _userService.LoginAsync(login);
            var jwtAuthenticationToken = _jwtTokenGeneratorForAuthentication.GenerateTokenForUser(loggedInUser);

            return Ok(jwtAuthenticationToken);
        }
    }
}
