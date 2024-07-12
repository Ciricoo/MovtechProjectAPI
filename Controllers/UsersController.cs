using Microsoft.AspNetCore.Mvc;
using MovtechProject.Models;
using MovtechProject.Services;
using System.Collections.Concurrent;



namespace MovtechProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

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
            string token = await _userService.UserLogin(loginUser);
            return Ok(token);
        }
    }
}