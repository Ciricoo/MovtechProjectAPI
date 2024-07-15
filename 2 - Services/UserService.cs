﻿using Microsoft.IdentityModel.Tokens;
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
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            List<User> list = await _userRepository.GetUsersAsync();

            if (list == null)
            {
                throw new ArgumentException("Não existe nenhum usuário!");
            }

            return list;
        }

        public async Task<User> CreateUsersAsync(User users)
        {
            List<User> usuarios = await _userRepository.GetUsersAsync();

            if (usuarios.Any(u => u.Name == users.Name))
            {
                throw new ArgumentException("Já existe um usuário com esse nome!");
            }

            if (string.IsNullOrWhiteSpace(users.Name) || string.IsNullOrWhiteSpace(users.Password))
            {
                throw new ArgumentException("Usuario ou senha não podem ser vazios!");
            }

            if (!Enum.IsDefined(typeof(UserEnumType), users.Type))
            {
                throw new ArgumentException("Tipo de usuário inválido!");
            }
            return await _userRepository.CreateUsersAsync(users);
        }

        public async Task<string> UserLogin(User loginUser)
        {
            List<User> users = await _userRepository.GetUsersAsync();
            User? user = users.FirstOrDefault(u => u.Name == loginUser.Name && u.Password == loginUser.Password);

            if (user == null)
            {
                throw new ArgumentException("Credenciais inválidas");
            }

            var token = GenerateToken(loginUser);

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
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Encriptar e desencriptar o token
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
