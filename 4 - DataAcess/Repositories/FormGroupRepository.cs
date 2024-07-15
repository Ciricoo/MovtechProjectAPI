using MovtechProject.DataAcess.Data;
using MovtechProject.Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace MovtechProject.DataAcess.Repositories
{
    public class FormGroupRepository
    {
        private readonly Database _database;

        public FormGroupRepository(Database database)
        {
            _database = database;
        }

        public async Task<List<FormGroup>> GetFormsGroupsAsync()
        {
            List<FormGroup> formsGroup = new List<FormGroup>();

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
                            formsGroup.Add(new FormGroup
                            {
                                Id = (int)reader["id"],
                                Name = reader.GetString("nome"),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro não esperado ao obter grupos de formulários: ", ex.Message);
            }

            return formsGroup;
        }

        public async Task<FormGroup?> GetFormsGroupByIdAsync(int id)
        {
            FormGroup? formsGroup = null;
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
                            formsGroup = new FormGroup
                            {
                                Id = (int)reader["id"],
                                Name = reader.GetString("nome")
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro não esperado ao obter o grupo de formulário por ID:", ex.Message);
            }

            return formsGroup;
        }

        public async Task<FormGroup> CreateFormsGroupAsync(FormGroup formsGroup)
        {
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("INSERT INTO grupoFormulario (nome) VALUES (@nome); SELECT SCOPE_IDENTITY();", connection);
                    command.Parameters.AddWithValue("@nome", formsGroup.Name);

                    var insertedId = await command.ExecuteScalarAsync();
                    formsGroup.Id = Convert.ToInt32(insertedId);
                    return formsGroup;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro não esperado ao criar o grupo de formulários:", ex.Message);
            }
        }

        public async Task<bool> UpdateFormsGroupAsync(int id, FormGroup formsGroup)
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
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro não esperado ao atualizar o grupo de formulários:", ex.Message);
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
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro não esperado ao excluir grupo de formulários:", ex.Message);
            }
        }
    }
}
