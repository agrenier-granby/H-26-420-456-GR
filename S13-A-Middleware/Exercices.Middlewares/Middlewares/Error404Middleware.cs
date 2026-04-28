namespace Exercices.Middlewares.Middlewares
{
    public class Error404Middleware
    {
        private readonly RequestDelegate _next;

        public Error404Middleware(RequestDelegate next)
        {
            // C'est une référence vers le prochain middleware!
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Appel du prochain middleware dans la liste
            await _next(context);// Laisse la requête poursuivre son cours vers les autres middlewares et atteindra le contrôleur

            if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
            {
                //Re-execute the request so the user gets the error 404 page
                string? originalPath = context.Request.Path.Value;
                context.Items["originalPath"] = originalPath;
                context.Request.Path = "/Home/Erreur404";
                await _next(context);
            }
        }
    }
}
