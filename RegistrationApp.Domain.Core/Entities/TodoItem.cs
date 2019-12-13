using System;
using RegistrationApp.Domain.Core.Entities.Identity;

namespace RegistrationApp.Domain.Core.Entities
{
    public class TodoItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsDone { get; set; }

        public DateTime? DateAdded { get; set; }

        public DateTime? DateDone { get; set; }

        public User AddedBy { get; set; }
    }
}
