using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using RegistrationApp.Domain.Core.Exceptions;

namespace RegistrationApp.Api.Utilities.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                var message = "Internal server error";

                if (exception is EntityNotFoundException || exception is ArgumentNullException || exception is DomainException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    message = exception.Message;
                }

                if (exception is AuthenticationException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    message = exception.Message;
                }

                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }

                await context.Response.WriteAsync(message);
            }
        }
    }
}
