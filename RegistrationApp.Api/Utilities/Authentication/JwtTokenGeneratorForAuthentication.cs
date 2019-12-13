using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RegistrationApp.Domain.Core.Entities.Identity;

namespace RegistrationApp.Api.Utilities.Authentication
{
    public class JwtTokenGeneratorForAuthentication
    {
        private readonly AuthenticationParameters _authenticationParameters;

        public JwtTokenGeneratorForAuthentication(IOptions<AuthenticationParameters> authenticationParameters)
        {
            _authenticationParameters = authenticationParameters.Value;
        }

        public string GenerateTokenForUser(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecretString = _authenticationParameters.JwtSecretString;
            var key = Encoding.ASCII.GetBytes(jwtSecretString);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.Name)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
