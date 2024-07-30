using Microsoft.IdentityModel.Tokens;
using MovtechProject.Domain.Models;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenCommandHandlers
{
    private readonly ConcurrentDictionary<string, int> _activeTokens = new ConcurrentDictionary<string, int>();
    private static readonly HashSet<string> RevokedTokens = new HashSet<string>();

    public string GenerateToken(User loginUser)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("43e4dbf0-52ed-4203-895d-42b586496bd4");

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, loginUser.Name),
                new Claim(ClaimTypes.Role, loginUser.Type.ToString()),
                new Claim(ClaimTypes.NameIdentifier, loginUser.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        string tokenString = tokenHandler.WriteToken(token);

        _activeTokens[tokenString] = loginUser.Id;

        return tokenString;
    }

    public void TokenValidation(string token)
    {
        RevokedTokens.Add(token);
    }

    public bool IsTokenRevoked(string token)
    {
        return RevokedTokens.Contains(token);
    }

    public async Task RevokeToken(HttpContext httpContext)
    {
        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token == null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsync("Precisa efetuar o login");
            return;
        }

        TokenValidation(token);

        _activeTokens.Remove(token, out _);
    }

    public bool IsUserLoggedIn(int userId)
    {
        bool isLoggedIn = _activeTokens.Values.Contains(userId);

        return isLoggedIn;
    }


}
