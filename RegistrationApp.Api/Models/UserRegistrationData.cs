using RegistrationApp.Domain.Core.Entities.Identity;

namespace RegistrationApp.Api.Models
{
    public class UserRegistrationData
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string RepeatPassword { get; set; }

        public Role Role { get; set; }
    }
}
