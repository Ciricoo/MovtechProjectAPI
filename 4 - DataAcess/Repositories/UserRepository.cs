using System.Data.SqlClient;
using System.Data;
using MovtechProject.Domain.Models;
using MovtechProject.Domain.Models.Enums;
using MovtechProject.DataAcess.Data;

namespace MovtechProject.DataAcess.Repositories
{
    public class UserRepository
    {
        private readonly Database _database;

        public UserRepository(Database database)
        {
            _database = database;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            List<User> Users = new List<User>();

            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM usuarios", connection);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Users.Add(new User
                        {
                            Id = (int)reader["id"],
                            Name = reader.GetString("nome"),
                            Password = reader.GetString("senha"),
                            Type = Enum.Parse<UserEnumType>(reader.GetString("tipo"))
                        });
                    }
                }
            }

            return Users;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            User? Users = null;

            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM usuarios WHERE @id = id", connection);
                command.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Users = new User
                        {
                            Id = (int)reader["id"],
                            Name = reader.GetString("nome"),
                            Password = reader.GetString("senha"),
                            Type = Enum.Parse<UserEnumType>(reader.GetString("tipo"))
                        };
                    }
                }
            }

            return Users;
        }

        public async Task<User> CreateUsersAsync(User users)
        {
            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("INSERT INTO usuarios (nome, senha, tipo) VALUES (@nome, @senha, @tipo); SELECT SCOPE_IDENTITY();", connection);
                command.Parameters.AddWithValue("@nome", users.Name);
                command.Parameters.AddWithValue("@senha", users.Password);
                command.Parameters.AddWithValue("@tipo", users.Type.ToString());

                var insertedId = await command.ExecuteScalarAsync();
                users.Id = Convert.ToInt32(insertedId);

                return users;
            }
        }
    }
}
