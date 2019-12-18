using Microsoft.AspNetCore.Builder;
using RegistrationApp.Api.Utilities.Middleware;

namespace RegistrationApp.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void UseExceptionsMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
