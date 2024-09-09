
using Microsoft.Extensions.Primitives;

namespace MovtechProject._3___Domain.Middleware
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, TokenCommandHandlers tokenHandlers)
        {
            string path = httpContext.Request.Path.Value!.ToLower();

            if (path == "/api/user/login" || path == "/api/user/logout" || path == "/api/user/")
            {
                await _next(httpContext);
                return;
            }

            string token = httpContext.Request.Headers.Authorization.FirstOrDefault()!.Split(" ").Last();
             string refreshToken = httpContext.Request.Cookies["Refresh-Token"]!;

            if (string.IsNullOrEmpty(token) || tokenHandlers.IsTokenRevoked(token) || tokenHandlers.IsTokenExpires(token))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized; return;
            }

            if (!string.IsNullOrEmpty(refreshToken) && tokenHandlers.ValidateRefreshToken(refreshToken, out string newJwtToken, out string newRefreshToken))
            {
                httpContext.Response.OnStarting(() =>
                {
                    httpContext.Response.Headers.Add("Authorization", $"Bearer {newJwtToken}");
                    httpContext.Response.Cookies.Append("Refresh-Token", newRefreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None, // Permite que o cookie seja enviado em requisições cross-site
                    });

                    return Task.CompletedTask;
                });
            }

            await _next(httpContext);
        }
    }
}
