﻿using MovtechProject.Data;
using MovtechProject.Models;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MovtechProject.Services
{
    public class FormsService
    {
        private readonly Database _database;

        public FormsService(Database database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
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
                                Name = reader["nome"].ToString(),
                                IdFormsGroup = (int)reader["idGrupoFormulario"]
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


            return Forms;
        }

        public async Task<Forms> GetFormularioByIdAsync(int id)
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
                                Name = reader["nome"].ToString(),
                                IdFormsGroup = (int)reader["idGrupoFormulario"]
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao obter formulário por ID:", ex.Message);
                throw;
            }
            return Forms;
        }

        public async Task<int> CreateFormsAsync(Forms forms)
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
                    return Convert.ToInt32(insertedId);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao criar formulário:", ex.Message);
                throw;
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
                Console.WriteLine("Erro não esperado ao atualizar formulário:", ex.Message);
                throw;
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
            }catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao excluir formulário:", ex.Message);
                throw;
            }
        }
    }
}

