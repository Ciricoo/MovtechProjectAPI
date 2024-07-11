﻿using MovtechProject.Data;
using MovtechProject.Models;
using System.Data;
using System.Data.SqlClient;

namespace MovtechProject.Repositories
{
    public class FormsRepository
    {
        private readonly Database _database;

        public FormsRepository(Database database)
        {
            _database = database;
        }

        public async Task<List<Forms>> GetFormsAsync()
        {
            List<Forms> Forms = new List<Forms>();

            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM formulario", connection);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Forms.Add(new Forms
                            {
                                Id = (int)reader["id"],
                                Name = reader.GetString("nome"),
                                IdFormsGroup = (int)reader["idGrupoFormulario"],
                                Questions = new List<Questions>()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro não esperado ao obter formulários: ", ex.Message);  
            }
            return Forms;
        }

        public async Task<Forms?> GetFormsByIdAsync(int id)
        {
            Forms? Forms = null;
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM formulario WHERE id = @id", connection);
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Forms = new Forms
                            {
                                Id = (int)reader["id"],
                                Name = reader.GetString("nome"),
                                IdFormsGroup = (int)reader["idGrupoFormulario"],
                                Questions = new List<Questions>()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro não esperado ao obter formulário por ID:", ex.Message);
            }
            return Forms;
        }

        public async Task<Forms> CreateFormsAsync(Forms forms)
        {
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("INSERT INTO formulario (nome, idGrupoFormulario) VALUES (@nome, @idGrupoFormulario); SELECT SCOPE_IDENTITY();", connection);
                    command.Parameters.AddWithValue("@nome", forms.Name);
                    command.Parameters.AddWithValue("@idGrupoFormulario", forms.IdFormsGroup);

                    var insertedId = await command.ExecuteScalarAsync();

                    Forms Forms = new Forms
                    {
                        Id = Convert.ToInt32(insertedId),
                        Name = forms.Name,
                        IdFormsGroup = forms.IdFormsGroup,
                    };

                    return Forms;

                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro não esperado ao criar formulário:", ex.Message);
            }
        }

        public async Task<bool> UpdateFormsAsync(int id, Forms forms)
        {
            try
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
            catch (Exception ex)
            {
                throw new ArgumentException("Erro não esperado ao atualizar formulário:", ex.Message);
            }
        }

        public async Task<bool> DeleteFormsAsync(int id)
        {
            try
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
            catch (Exception ex)
            {
                throw new ArgumentException("Erro não esperado ao excluir formulário:", ex.Message);
            }
        }

        public async Task<List<Forms>> GetFormsByGroupId(int idGroup)
        {
            List<Forms> Forms = new List<Forms>();

            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM formulario WHERE @idGrupoFormulario = idGrupoFormulario", connection);
                    command.Parameters.AddWithValue("@idGrupoFormulario", idGroup);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Forms.Add(new Forms
                            {
                                Id = (int)reader["id"],
                                Name = reader.GetString("nome"),
                                IdFormsGroup = (int)reader["idGrupoFormulario"],
                                Questions = new List<Questions>()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Erro não esperado ao obter o id do grupo de formularios: ", ex.Message);
            }
            return Forms;
        }
    }
}
