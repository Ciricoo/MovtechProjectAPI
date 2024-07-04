using Microsoft.AspNetCore.Mvc;
using MovtechProject.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Users loginRequest)
    {
        var user = await _userService.AuthenticateAsync(loginRequest.Name, loginRequest.Password);
        if (user == null)
        {
            return Unauthorized(new { message = "Nome de usuário ou senha incorretos" });
        }

        // Cria um objeto simples para armazenar o usuário autenticado
        return Ok(new { Id = user.Id, Name = user.Name, Type = user.Type });
    }
}
