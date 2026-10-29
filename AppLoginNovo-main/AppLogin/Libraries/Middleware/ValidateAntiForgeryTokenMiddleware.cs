using Microsoft.AspNetCore.Antiforgery;

namespace AppLogin.Libraries.Middleware
{
    public class ValidateAntiForgeryTokenMiddleware
    {
        private RequestDelegate _next;
        private IAntiforgery _antiforgery;

        public ValidateAntiForgeryTokenMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            _next = next;
            _antiforgery = antiforgery;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            // Ignora a documentação Swagger e as rotas da API
            if (path.StartsWithSegments("/swagger") ||
                path.StartsWithSegments("/api"))
            {
                await _next(context);
                return;
            }

            // Valida requisições POST das páginas tradicionais
            if (HttpMethods.IsPost(context.Request.Method))
            {
                await _antiforgery.ValidateRequestAsync(context);
            }

            await _next(context);
        }
    }
}
