using MovtechProject.DataAcess.Data;
using MovtechProject.Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace MovtechProject.DataAcess.Repositories
{
    public class FormRepository
    {
        private readonly Database _database;

        public FormRepository(Database database)
        {
            _database = database;
        }

        public async Task<List<Form>> GetFormsAsync()
        {
            List<Form> Forms = new List<Form>();

            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM formulario", connection);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Forms.Add(new Form
                        {
                            Id = (int)reader["id"],
                            Name = reader.GetString("nome"),
                            IdFormsGroup = (int)reader["idGrupoFormulario"],
                        });
                    }
                }
            }
            return Forms;
        }

        public async Task<Form?> GetFormsByIdAsync(int id)
        {
            Form? Forms = null;

            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM formulario WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        Forms = new Form
                        {
                            Id = (int)reader["id"],
                            Name = reader.GetString("nome"),
                            IdFormsGroup = (int)reader["idGrupoFormulario"],
                        };
                    }
                }
            }

            return Forms;
        }

        public async Task<Form> CreateFormsAsync(Form forms)
        {
            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("INSERT INTO formulario (nome, idGrupoFormulario) VALUES (@nome, @idGrupoFormulario); SELECT SCOPE_IDENTITY();", connection);
                command.Parameters.AddWithValue("@nome", forms.Name);
                command.Parameters.AddWithValue("@idGrupoFormulario", forms.IdFormsGroup);

                var insertedId = await command.ExecuteScalarAsync();
                forms.Id = Convert.ToInt32(insertedId);

                return forms;

            }
        }

        public async Task<bool> UpdateFormsAsync(int id, Form forms)
        {
            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("UPDATE formulario SET nome = @nome, idGrupoFormulario = @idGrupoFormulario WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@nome", forms.Name);
                command.Parameters.AddWithValue("@idGrupoFormulario", forms.IdFormsGroup);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;

            }
        }

        public async Task<bool> DeleteFormsAsync(int id)
        {

            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("DELETE FROM formulario WHERE @id = id", connection);
                command.Parameters.AddWithValue("@id", id);               

                int rowAffected = await command.ExecuteNonQueryAsync();
                return rowAffected > 0;
            }
        }

        public async Task<List<Form>> GetFormsByGroupId(int idGroup)
        {
            List<Form> Forms = new List<Form>();

            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM formulario WHERE @idGrupoFormulario = idGrupoFormulario", connection);
                command.Parameters.AddWithValue("@idGrupoFormulario", idGroup);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Forms.Add(new Form
                        {
                            Id = (int)reader["id"],
                            Name = reader.GetString("nome"),
                            IdFormsGroup = (int)reader["idGrupoFormulario"],
                        });
                    }
                }
            }
            return Forms;
        }
    }
}
