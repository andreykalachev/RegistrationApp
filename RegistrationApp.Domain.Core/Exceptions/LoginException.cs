using System;

namespace RegistrationApp.Domain.Core.Exceptions
{
    public class LoginException : Exception
    {
        public LoginException(string message) : base(message)
        {
        }
    }
}
