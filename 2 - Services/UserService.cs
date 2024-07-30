using MovtechProject.Domain.Models;

public class UserService
{
    private readonly UserCommandHandlers _userCommandHandlers;
    private readonly TokenCommandHandlers _tokenCommandHandlers;

    public UserService(UserCommandHandlers userCommandHandlers, TokenCommandHandlers tokenCommandHandlers)
    {
        _userCommandHandlers = userCommandHandlers;
        _tokenCommandHandlers = tokenCommandHandlers;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _userCommandHandlers.GetUsersAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _userCommandHandlers.GetUserByIdAsync(id);
    }

    public async Task<User> CreateUsersAsync(User users)
    {
        return await _userCommandHandlers.CreateUsersAsync(users);
    }

    public async Task<string> UserLogin(User loginUser)
    {
        return await _userCommandHandlers.UserLogin(loginUser);
    }
    public async Task Logout(HttpContext token)
    {
        await _tokenCommandHandlers.RevokeToken(token);
    }
}
