using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovtechProject.Domain.Models;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = "Administrador")]
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        List<User> get = await _userService.GetUsersAsync();
        return Ok(get);
    }

    [Authorize(Roles = "Administrador")]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        User getId = await _userService.GetUserByIdAsync(id);
        return Ok(getId);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUsers(User users)
    {
        User created = await _userService.CreateUsersAsync(users);
        return Ok(created);
    }

    [HttpPost("login")]
    public async Task<ActionResult<(string, string)>> Login(User loginUser)
    {
        var (token, refreshToken) = await _userService.UserLogin(loginUser);
        return Ok(new { token, refreshToken });
    }

    [HttpPost("logout")]
    public ActionResult Logout()
    {
        _userService.Logout(HttpContext);
        return Ok("Logout concluido");
    }

    [HttpPost("refresh")]
    public ActionResult RefreshToken([FromBody] string refreshToken)
    {
        _userService.ValidateRefreshToken(refreshToken, out string newJwtToken, out string newRefreshToken);
        return Ok(new { token = newJwtToken, refreshToken = newRefreshToken });
    }
}
