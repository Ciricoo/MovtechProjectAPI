using Microsoft.AspNetCore.Mvc;
using MovtechProject.Domain.Models;
using MovtechProject.Services;
using System.Collections.Concurrent;



namespace MovtechProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            List<User> users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUsers(User users)
        {
            User created = await _userService.CreateUsersAsync(users);
            return Ok(created);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(User loginUser)
        {
            string token = await _userService.UserLogin(loginUser);
            return Ok(token);
        }
    }
}