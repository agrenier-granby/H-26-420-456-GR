using Microsoft.AspNetCore.Http.Extensions;
using System.Diagnostics;

namespace Exercices.Middlewares.Middlewares
{
    public class PerformanceProfiler(ILogger<PerformanceProfiler> logger, RequestDelegate next)
    {
        private const string LOG_MESSAGE = "Requête du thread [{threadId}] vers [{pathAndQuery}] exécutée en [{timeMs}] ms.";
        private readonly RequestDelegate _next = next;
        private readonly ILogger<PerformanceProfiler> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();
            try
            {
                // Appel du prochain middleware dans la liste
                await _next(context);// Laisse la requête poursuivre son cours vers les autres middlewares et atteindra le contrôleur

            }
            finally
            {
                watch.Stop();
                var threadId = Environment.CurrentManagedThreadId;
                var pathAndQuery = context.Request.GetEncodedPathAndQuery();
                var timeMs = watch.ElapsedMilliseconds;
                _logger.LogInformation(LOG_MESSAGE, threadId, pathAndQuery, timeMs);
            }
        }
    }
}
