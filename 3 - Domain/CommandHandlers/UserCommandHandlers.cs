using Microsoft.IdentityModel.Tokens;
using MovtechProject._3___Domain.CommandHandlers;
using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models.Enums;
using MovtechProject.Domain.Models;

public class UserCommandHandlers
{
    private readonly UserRepository _userRepository;
    private readonly TokenCommandHandlers _tokenCommandHandlers;

    public UserCommandHandlers(UserRepository userRepository, TokenCommandHandlers tokenCommandHandlers)
    {
        _userRepository = userRepository;
        _tokenCommandHandlers = tokenCommandHandlers;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        List<User> list = await _userRepository.GetUsersAsync();

        if (!list.Any())
        {
            throw new ArgumentNullException("Não existe nenhum usuário!");
        }

        return list;
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
        List<User> usuarios = await _userRepository.GetUsersAsync();

        if (usuarios.Any(u => u.Name == users.Name))
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

    public async Task<string> UserLogin(User loginUser)
    {
        List<User> users = await _userRepository.GetUsersAsync();
        User? user = users.FirstOrDefault(u => u.Name == loginUser.Name && u.Password == loginUser.Password);

        if (user == null)
        {
            throw new InvalidOperationException("Credenciais inválidas");
        }

        if (_tokenCommandHandlers.IsUserLoggedIn(user.Id))
        {
            throw new InvalidOperationException("O usuário já está logado.");
        }

        string token = _tokenCommandHandlers.GenerateToken(user);
        return token;
    }
}
