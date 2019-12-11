using System;
using System.Collections.Generic;

namespace RegistrationApp.Domain.Core.Identity
{
    public class Role
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IList<User> Users { get; set; }

        public Role()
        {
            Users = new List<User>();
        }
    }
}
