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
                                Type = Enum.Parse<UserEnumType>(reader.GetString("tipo"))

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
                    command.Parameters.AddWithValue("@tipo", users.Type);

                    var insertedId = await command.ExecuteScalarAsync();
                    users.Id = Convert.ToInt32(insertedId);
                    return users;

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
