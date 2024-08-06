﻿using Microsoft.IdentityModel.Tokens;
using MovtechProject.Domain.Models;
using MovtechProject.Domain.Models.Enums;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenCommandHandlers
{
    string? activeToken;
    private static readonly HashSet<string> RevokedTokens = new HashSet<string>();
    private readonly ConcurrentDictionary<string, string> _refreshTokens = new ConcurrentDictionary<string, string>();

    public string GenerateToken(User loginUser, out string refreshToken)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes("43e4dbf0-52ed-4203-895d-42b586496bd4");

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, loginUser.Name),
                new Claim(ClaimTypes.Role, loginUser.Type.ToString()),
                new Claim(ClaimTypes.NameIdentifier, loginUser.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)// HmacSha256Signature valida a autenticidade do token
            // SymmetricSecurityKey(key) cria uma nova chave de segurança simétrica usando o valor de key.
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        string tokenString = tokenHandler.WriteToken(token);

        activeToken = tokenString;

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
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(oldJwtToken))
            {
                JwtSecurityToken jwtToken = handler.ReadJwtToken(oldJwtToken);

                activeToken = null;

                RevokedTokens.Add(oldJwtToken);

                string nameClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "unique_name")!.Value;
                string roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role")!.Value;
                string nameIdentifierClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid")!.Value;

                int nameId = Convert.ToInt32(nameIdentifierClaim);

                if (nameClaim != null && roleClaim != null && nameIdentifierClaim != null)
                {
                    User user = new User { Id = nameId, Name = nameClaim!, Type = Enum.Parse<UserEnumType>(roleClaim!) };
                    newJwtToken = GenerateToken(user, out newRefreshToken);
                    _refreshTokens.Remove(refreshToken, out _);
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsTokenRevoked(string token)
    {
        return RevokedTokens.Contains(token);
    }

    public void RevokeToken(HttpContext httpContext)
    {
        string token = httpContext.Request.Headers["Authorization"].FirstOrDefault()!.Split(" ").Last();

        RevokedTokens.Add(token);

        activeToken = null;
    }

    public bool IsUserLoggedIn()
    {
        return activeToken != null;
    }
}
