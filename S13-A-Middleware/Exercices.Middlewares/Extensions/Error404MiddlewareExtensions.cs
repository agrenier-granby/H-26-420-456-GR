using Exercices.Middlewares.Middlewares;

namespace Exercices.Middlewares.Extensions
{
    public static class Error404MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomError404PageMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Error404Middleware>();
        }
    }
}
