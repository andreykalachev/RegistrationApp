using System;

namespace RegistrationApp.Api.Models
{
    public class TodoItemUpdateViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
