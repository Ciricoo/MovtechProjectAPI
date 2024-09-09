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

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _userCommandHandlers.GetUserByIdAsync(id);
    }

    public async Task<User> CreateUsersAsync(User users)
    {
        return await _userCommandHandlers.CreateUsersAsync(users);
    }

    public async Task<(string, string)> UserLogin(User loginUser, HttpContext httpContext)
    {
        return await _userCommandHandlers.UserLogin(loginUser, httpContext);
    }

    public void Logout(HttpContext httpContext)
    {
         _userCommandHandlers.Logout(httpContext);
    }

    public bool ValidateRefreshToken(string refreshToken, out string newJwtToken, out string newRefreshToken)
    {
        return _userCommandHandlers.ValidateRefreshToken(refreshToken, out newJwtToken, out newRefreshToken);
    }

    public async Task<List<User>> GetUserAsync()
    {
        return await _userCommandHandlers.GetUserAsync();
    }

    public async Task<List<int>> GetAnswersAccordingNpsGrade()
    {
        return await _userCommandHandlers.GetAnswersAccordingNpsGrade();
    }

    public void RevokeToken(HttpContext httpContext)
    {
        _tokenCommandHandlers.RevokeToken(httpContext);
    }

}
