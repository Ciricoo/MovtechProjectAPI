using MovtechProject.Domain.Models;

public class UserService
{
    private readonly UserCommandHandlers _userCommandHandlers;

    public UserService(UserCommandHandlers userCommandHandlers)
    {
        _userCommandHandlers = userCommandHandlers;
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

    public async Task<(string, string)> UserLogin(User loginUser)
    {
        return await _userCommandHandlers.UserLogin(loginUser);
    }

    public void Logout(HttpContext httpContext)
    {
         _userCommandHandlers.Logout(httpContext);
    }

    public bool ValidateRefreshToken(string refreshToken, out string newJwtToken, out string newRefreshToken)
    {
        return _userCommandHandlers.ValidateRefreshToken(refreshToken, out newJwtToken, out newRefreshToken);
    }
}
