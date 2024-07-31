using MovtechProject.Domain.Models;

namespace MovtechProject._3___Domain.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, TokenCommandHandlers RevokeToken)
        {
            var path = httpContext.Request.Path.Value?.ToLower();

            if (path == "/api/user/login" || path == "/api/user/")
            {
                await _next(httpContext);
                return;
            } 

            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if(token != null && RevokeToken.IsTokenRevoked(token))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized; return;
            }

            await _next(httpContext);
        }
    }
}
