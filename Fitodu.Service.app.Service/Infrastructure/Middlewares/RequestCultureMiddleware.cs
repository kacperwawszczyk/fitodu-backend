using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Fitodu.Core.Helpers;

namespace Fitodu.Service.Infrastructure.Middlewares
{
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string language = context.Request.Headers["Language"];

            Language.SetLanguage(language);

            await _next(context);
        }
    }
}