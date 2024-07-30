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
            return formsGroup;
        }

        public async Task<FormGroup?> GetFormsGroupByIdAsync(int id)
        {
            FormGroup? formsGroup = null;

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
            return formsGroup;
        }

        public async Task<FormGroup> CreateFormsGroupAsync(FormGroup formsGroup)
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

        public async Task<bool> UpdateFormsGroupAsync(int id, FormGroup formsGroup)
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

        public async Task<bool> DeleteFormsGroupAsync(int id)
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
    }
}
