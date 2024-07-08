using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovtechProject.Models;
using MovtechProject.Services;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var users = await _userService.GetUsersAsync();
            var user = users.FirstOrDefault(u => u.Name == loginUser.Name && u.Password == loginUser.Password);

            if (user == null)
            {
                return Unauthorized("Credenciais inválidas.");
            }

            string token = System.Guid.NewGuid().ToString();
            _loggedInUsers[token] = user.Type.ToString();

            return Ok(token);
        }

        public static bool TryGetUserType(string token, out string userType)
        {
            return _loggedInUsers.TryGetValue(token, out userType);
        }
    }
}
