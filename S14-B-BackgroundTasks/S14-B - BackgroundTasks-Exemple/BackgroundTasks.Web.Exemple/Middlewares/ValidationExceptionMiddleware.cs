using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundTasks.Web.Exemple.Middlewares
{
    // TODO: register this middleware in the program.cs
    public class ValidationExceptionMiddleware(RequestDelegate request)
    {
        private readonly RequestDelegate _request = request;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _request(context);
            }
            catch (ValidationException exception)
            {
                context.Response.StatusCode = 400;

                var error = new ValidationProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Status = 400,
                    Extensions =
                {
                    ["traceId"] = context.TraceIdentifier
                }
                };
                // Usage of Nuget FluentValidation
                foreach (var validationFailure in exception.Errors)
                {
                    error.Errors.Add(new KeyValuePair<string, string[]>(
                        validationFailure.PropertyName,
                        [validationFailure.ErrorMessage]));
                }
                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
