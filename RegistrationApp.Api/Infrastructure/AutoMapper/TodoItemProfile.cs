using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RegistrationApp.Api.Models;
using RegistrationApp.Domain.Core.Entities;

namespace RegistrationApp.Api.Infrastructure.AutoMapper
{
    public class TodoItemProfile : Profile
    {
        public TodoItemProfile()
        {
            CreateMap<TodoItemPostViewModel, TodoItem>();
        }
    }
}
