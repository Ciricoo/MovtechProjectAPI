using Microsoft.SqlServer.Server;
using MovtechProject.Data;
using MovtechProject.Models;
using MovtechProject.Models.Enums;
using System.Data;
using System.Data.SqlClient;

namespace MovtechProject.Services
{
    public class UserService
    {
        private readonly Database _database;
        public UserService(Database database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<List<Users>> GetUsersAsync()
        {
            List<Users> Users = new List<Users>();
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
                            Users.Add(new Users
                            {
                                Id = (int)reader["id"],
                                Name = reader.GetString("nome"),
                                Password = reader.GetString("senha"),
                                Type = Enum.Parse < UserEnumType >(reader.GetString("tipo"))

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao obter formulários: ", ex.Message);
                throw;
            }


            return Users;
        }

        public async Task<int> CreateUsersAsync(Users users)
        {
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("INSERT INTO usuarios (nome, senha, tipo) VALUES (@nome, @senha, @tipo); SELECT SCOPE_IDENTITY();", connection);
                    command.Parameters.AddWithValue("@nome", users.Name);
                    command.Parameters.AddWithValue("@senha", users.Password);
                    command.Parameters.AddWithValue("@tipo", users.Type);

                    var insertedId = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(insertedId);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao criar formulário:", ex.Message);
                throw;
            }
        }
    }
}
