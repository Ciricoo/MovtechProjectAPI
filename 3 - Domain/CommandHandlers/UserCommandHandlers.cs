using Microsoft.AspNetCore.Http;
using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;
using MovtechProject.Domain.Models.Enums;

public class UserCommandHandlers
{
    private readonly UserRepository _userRepository;
    private readonly TokenCommandHandlers _tokenCommandHandlers;
    private readonly AnswerRepository _answerRepository;

    public UserCommandHandlers(UserRepository userRepository, TokenCommandHandlers tokenCommandHandlers, AnswerRepository answerRepository)
    {
        _userRepository = userRepository;
        _tokenCommandHandlers = tokenCommandHandlers;
        _answerRepository = answerRepository;
    }

    public async Task<List<User>> GetUserAsync()
    {
        List<User> users =  await _userRepository.GetUserAsync();
        if (users == null)
        {
            throw new ArgumentNullException("Não existem usuários!");
        }
        return users;
    }
    public async Task<User> GetUserByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("ID inválido!", nameof(id));
        }

        User? users = await _userRepository.GetUserByIdAsync(id);

        if (users == null)
        {
            throw new KeyNotFoundException($"Id {id} não encontrado!");
        }

        return users;
    }

    public async Task<User> CreateUsersAsync(User users)
    {
        List<User> usuarios = await _userRepository.GetUserByNameAsync (users.Name);

        if (usuarios.Any())
        {
            throw new ArgumentException("Já existe um usuário com esse nome!", users.Name);
        }

        if (string.IsNullOrWhiteSpace(users.Name) || string.IsNullOrWhiteSpace(users.Password))
        {
            throw new InvalidOperationException("Usuario ou senha não podem ser vazios!");
        }

        if (!Enum.IsDefined(typeof(UserEnumType), users.Type))
        {
            throw new ArgumentException("Tipo de usuário inválido!", nameof(users.Type));
        }
        return await _userRepository.CreateUsersAsync(users);
    }

    public async Task<(string token, string refreshToken)> UserLogin(User loginUser, HttpContext httpContext)
    {
        List<User> users = await _userRepository.GetUserByNameAsync(loginUser.Name);
        User? user = users.FirstOrDefault(u => u.Password == loginUser.Password);

        if (_tokenCommandHandlers.IsUserLoggedIn())
        {
            throw new InvalidOperationException("O usuário já está logado.");
        }

        if (user == null)
        {
            throw new InvalidOperationException("Credenciais inválidas");
        }


        string token = _tokenCommandHandlers.GenerateToken(user, out string refreshToken);

        httpContext.Response.Headers.Add("Authorization", $"Bearer {token}");
        httpContext.Response.Cookies.Append("Refresh-Token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Necessário para HTTPS
            SameSite = SameSiteMode.None, // Permite que o cookie seja enviado em requisições cross-site
        });

        return (token, refreshToken);
    }

    public void Logout(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete("Refresh-Token");
        _tokenCommandHandlers.RevokeToken(httpContext);
    }

    public bool ValidateRefreshToken(string refreshToken, out string newJwtToken, out string newRefreshToken)
    {
        bool validateRefresh = _tokenCommandHandlers.ValidateRefreshToken(refreshToken, out newJwtToken, out newRefreshToken);

        if (!validateRefresh)
        {
            throw new ArgumentException("Token de refresh inválido");
        }

        return validateRefresh;
    }

    

}
