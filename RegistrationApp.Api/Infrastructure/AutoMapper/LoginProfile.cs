using AutoMapper;
using RegistrationApp.Api.Models;
using RegistrationApp.Domain.Core.Entities.Identity;

namespace RegistrationApp.Api.Infrastructure.AutoMapper
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<UserRegistrationData, User>();
        }
    }
}
