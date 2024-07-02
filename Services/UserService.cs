using Microsoft.SqlServer.Server;
using MovtechProject.Data;
using MovtechProject.Models;
using MovtechProject.Models.Enums;
using MovtechProject.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace MovtechProject.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<Users>> GetUsersAsync()
        {
            var lista = _userRepository.GetUsersAsync();

            if (lista == null)
            {
                throw new ArgumentException("Não existe nenhum usuário!");
            }

            return await _userRepository.GetUsersAsync();
        }

        public async Task<Users> CreateUsersAsync(Users users)
        {
            if (string.IsNullOrWhiteSpace(users.Name) || string.IsNullOrWhiteSpace(users.Password))
            {
                throw new ArgumentException("Usuario ou senha não podem ser vazios!");
            }

            return await _userRepository.CreateUsersAsync(users);   
        }
    }
}
