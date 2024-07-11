using Microsoft.IdentityModel.Tokens;
using MovtechProject.Models;
using MovtechProject.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            List<Users> list = await _userRepository.GetUsersAsync();

            if (list == null)
            {
                throw new ArgumentException("Não existe nenhum usuário!");
            }

            return list;
        }

        public async Task<Users> CreateUsersAsync(Users users)
        {
            List<Users> usuarios = await _userRepository.GetUsersAsync();
            Users? user = usuarios.Find(u => users.Name == u.Name);

            if (user != null)
            {
                throw new ArgumentException("Já existe um usuário com esse nome!");
            }

            if (string.IsNullOrWhiteSpace(users.Name) || string.IsNullOrWhiteSpace(users.Password))
            {
                throw new ArgumentException("Usuario ou senha não podem ser vazios!");
            }
            return await _userRepository.CreateUsersAsync(users);
        }

        public async Task<string> GenerateToken(Users loginUser)
        {
            List<Users> users = await _userRepository.GetUsersAsync();
            Users? user = users.FirstOrDefault(u => u.Name == loginUser.Name && u.Password == loginUser.Password);

            if (user == null)
            {
                throw new ArgumentException("Credenciais inválidas");
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("43e4dbf0-52ed-4203-895d-42b586496bd4");

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Type.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

    }
}
