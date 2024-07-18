using Microsoft.IdentityModel.Tokens;
using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models.Enums;
using MovtechProject.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovtechProject._3___Domain.CommandHandlers
{
    public class UserCommandHandlers
    {
        private readonly UserRepository _userRepository;
        public UserCommandHandlers(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            List<User> list = await _userRepository.GetUsersAsync();

            if (!list.Any())
            {
                throw new ArgumentNullException("Não existe nenhum usuário!");
            }

            return list;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!", nameof(id));
            }

            User? users = await _userRepository.GetUserByIdAsync(id);

            if (users == null)
            {
                throw new KeyNotFoundException($"Id {id} não encontrado!");
            }

            return users;
        }

        public async Task<User> CreateUsersAsync(User users)
        {
            List<User> usuarios = await _userRepository.GetUsersAsync();

            if (usuarios.Any(u => u.Name == users.Name))
            {
                throw new ArgumentException("Já existe um usuário com esse nome!", users.Name);
            }

            if (string.IsNullOrWhiteSpace(users.Name) || string.IsNullOrWhiteSpace(users.Password))
            {
                throw new InvalidOperationException("Usuario ou senha não podem ser vazios!");
            }

            if (!Enum.IsDefined(typeof(UserEnumType), users.Type))
            {
                throw new ArgumentException("Tipo de usuário inválido!", nameof(users.Type));
            }
            return await _userRepository.CreateUsersAsync(users);
        }

        public async Task<string> UserLogin(User loginUser)
        {
            List<User> users = await _userRepository.GetUsersAsync();
            User? user = users.FirstOrDefault(u => u.Name == loginUser.Name && u.Password == loginUser.Password);

            if (user == null)
            {
                throw new InvalidOperationException("Credenciais inválidas");
            }

            string token = GenerateToken(user);

            return token;
        }

        public string GenerateToken(User loginUser)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler(); // Classe que gera o token
            var key = Encoding.ASCII.GetBytes("43e4dbf0-52ed-4203-895d-42b586496bd4"); // Transformando a chave em um array de bites

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor // Descreve informações importantes para o token funcionar
            {
                Subject = new ClaimsIdentity(new Claim[] // Coleção de afirmações sobre o usuário
                {
                    new Claim(ClaimTypes.Name, loginUser.Name),
                    new Claim(ClaimTypes.Role, loginUser.Type.ToString())
                   
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Encriptar e desencriptar o token
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
