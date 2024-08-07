using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;
using MovtechProject.Domain.Models.Enums;

public class UserCommandHandlers
{
    private readonly UserRepository _userRepository;
    private readonly TokenCommandHandlers _tokenCommandHandlers;

    public UserCommandHandlers(UserRepository userRepository, TokenCommandHandlers tokenCommandHandlers)
    {
        _userRepository = userRepository;
        _tokenCommandHandlers = tokenCommandHandlers;
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
        List<User> usuarios = await _userRepository.GetUserByLoginAsync(users.Name);

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

    public async Task<(string token, string refreshToken)> UserLogin(User loginUser)
    {
        List<User> users = await _userRepository.GetUserByLoginAsync(loginUser.Name);
        User? user = users.FirstOrDefault(u => u.Password == loginUser.Password);

        if (user == null)
        {
            throw new InvalidOperationException("Credenciais inválidas");
        }

        if (_tokenCommandHandlers.IsUserLoggedIn())
        {
            throw new InvalidOperationException("O usuário já está logado.");
        }

        string token = _tokenCommandHandlers.GenerateToken(user, out string refreshToken);
        return (token, refreshToken);
    }

    public void Logout(HttpContext httpContext)
    {
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
