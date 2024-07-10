using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovtechProject.Models;
using MovtechProject.Services;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace MovtechProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private static ConcurrentDictionary<string, string> _loggedInUsers = new ConcurrentDictionary<string, string>();

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> GetUsers()
        {
            List<Users> users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<Users>> CreateUsers(Users users)
        {
            Users created = await _userService.CreateUsersAsync(users);
            return Ok(created);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(Users loginUser)
        {
            var token = await _userService.GenerateToken(loginUser);
            return Ok(token);
        }
    }
}