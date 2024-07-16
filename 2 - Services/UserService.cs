using Microsoft.IdentityModel.Tokens;
using MovtechProject._3___Domain.CommandHandlers;
using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;
using MovtechProject.Domain.Models.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovtechProject.Services
{
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

        public async Task<string> UserLogin(User loginUser)
        {
            return await _userCommandHandlers.UserLogin(loginUser);
        }
    }
}
