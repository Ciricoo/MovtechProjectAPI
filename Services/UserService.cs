using MovtechProject.Models;
using MovtechProject.Repositories;

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
            var list = await _userRepository.GetUsersAsync();

            if (list == null)
            {
                throw new ArgumentException("Não existe nenhum usuário!");
            }

            return list;
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
