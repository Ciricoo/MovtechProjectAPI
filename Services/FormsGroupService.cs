using MovtechProject.Data;
using MovtechProject.Models;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace MovtechProject.Services
{
    public class FormsGroupService
    {
        private readonly Database _database;

        public FormsGroupService(Database database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<List<FormsGroup>> GetFormsGroupsAsync()
        {
            List<FormsGroup> formsGroup = new List<FormsGroup>();

            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM grupoFormulario", connection);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            formsGroup.Add(new FormsGroup
                            {
                                Id = (int)reader["id"],
                                Name = reader["nome"].ToString(),
                                
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao obter grupo de formulários: ", ex.Message);
                throw;
            }
            
            return formsGroup;
        }

        public async Task<FormsGroup> GetFormsGroupByIdAsync(int id)
        {
            FormsGroup formsGroup = null;

            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM grupoFormulario WHERE id = @id", connection);
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            formsGroup = new FormsGroup
                            {
                                Id = (int)reader["id"],
                                Name = reader["nome"].ToString()
                            };
                        }
                    }
                }
            }catch  (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao obter o grupo de formulário por ID:", ex.Message);
                throw;
            }
            
            return formsGroup;
        }

        public async Task<int> CreateFormsGroupAsync(FormsGroup formsGroup)
        {
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("INSERT INTO grupoFormulario (nome) VALUES (@nome); SELECT SCOPE_IDENTITY();", connection);
                    command.Parameters.AddWithValue("@nome", formsGroup.Name);

                    var insertedId = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(insertedId);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao criar o grupo de formulários:", ex.Message);
                throw;
            }
            
        }

       public async Task<bool> UpdateFormsGroupAsync(int id, FormsGroup formsGroup)
        {
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("UPDATE grupoFormulario SET nome = @nome WHERE id = @id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@nome", formsGroup.Name);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao atualizar o grupo de formulários:", ex.Message);
                throw;
            }
            
        }

        public async Task<bool> DeleteFormsGroupAsync(int id)
        {
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("DELETE FROM grupoFormulario WHERE id = @id", connection);
                    command.Parameters.AddWithValue("@id", id);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Erro não esperado ao excluir grupo de formulários:", ex.Message);
                throw;
            }
            
        }
    }
}
