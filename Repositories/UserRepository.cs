using MovtechProject.Data;
using MovtechProject.Models.Enums;
using MovtechProject.Models;
using System.Data.SqlClient;
using System.Data;

namespace MovtechProject.Repositories
{
    public class UserRepository
    {
        private readonly Database _database;

        public UserRepository(Database database)
        {
            _database = database;
        }

        public async Task<List<Users>> GetUsersAsync()
        {
            List<Users> users = new List<Users>();
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM usuarios", connection);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(new Users
                            {
                                Id = (int)reader["id"],
                                Name = reader.GetString("nome"),
                                Password = reader.GetString("senha"),
                                Type = Enum.TryParse<UserEnumType>(reader.GetString("tipo"), out var userType)
                                       ? userType
                                       : throw new InvalidCastException($"Valor inválido para enum UserEnumType: {reader.GetString("tipo")}")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro não esperado ao obter usuários: ", ex.Message);
            }

            return users;
        }

        public async Task<Users> CreateUsersAsync(Users users)
        {
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("INSERT INTO usuarios (nome, senha, tipo) VALUES (@nome, @senha, @tipo); SELECT SCOPE_IDENTITY();", connection);
                    command.Parameters.AddWithValue("@nome", users.Name);
                    command.Parameters.AddWithValue("@senha", users.Password);
                    command.Parameters.AddWithValue("@tipo", users.Type.ToString()); // Armazena o tipo como string

                    var insertedId = await command.ExecuteScalarAsync();
                    users.Id = Convert.ToInt32(insertedId);
                    return users;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro não esperado ao criar usuário:", ex.Message);
            }
        }
    }
}
