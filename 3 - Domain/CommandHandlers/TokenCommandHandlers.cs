using Microsoft.IdentityModel.Tokens;
using MovtechProject.Domain.Models;
using MovtechProject.Domain.Models.Enums;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenCommandHandlers
{
    private readonly List<string> _activeTokens = new List<string>();
    private static readonly HashSet<string> RevokedTokens = new HashSet<string>();
    private readonly ConcurrentDictionary<string, string> _refreshTokens = new ConcurrentDictionary<string, string>();

    public string GenerateToken(User loginUser, out string refreshToken)
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
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        string tokenString = tokenHandler.WriteToken(token);

        _activeTokens.Add(tokenString);

        refreshToken = Guid.NewGuid().ToString();

        _refreshTokens[refreshToken] = tokenString;

        return tokenString;
    }

    public bool ValidateRefreshToken(string refreshToken, out string newJwtToken, out string newRefreshToken)
    {
        newJwtToken = null!;
        newRefreshToken = null!;

        if (_refreshTokens.TryGetValue(refreshToken, out string oldJwtToken))
        {
            var handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(oldJwtToken))
            {
                var jwtToken = handler.ReadJwtToken(oldJwtToken);

                if (_activeTokens.Remove(oldJwtToken))
                {
                    RevokedTokens.Add(oldJwtToken);

                    var nameClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "unique_name")?.Value;
                    var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role")?.Value;
                    var nameIdentifierClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;

                    int nameId = Convert.ToInt32(nameIdentifierClaim);

                    if (nameClaim != null && roleClaim != null && nameIdentifierClaim != null);
                    {
                        User user = new User { Id = nameId, Name = nameClaim, Type = Enum.Parse<UserEnumType>(roleClaim) };
                        newJwtToken = GenerateToken(user, out newRefreshToken);
                        _refreshTokens.Remove(refreshToken, out _);
                        return true;
                    }
                }
            }
        }

        return false;
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

        RevokedTokens.Add(token);

        _activeTokens.Clear();
    }

    public bool IsUserLoggedIn()
    {
        return _activeTokens.Count > 0;
    }
}
