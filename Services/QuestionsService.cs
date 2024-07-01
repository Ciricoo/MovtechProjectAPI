using MovtechProject.Data;
using MovtechProject.Models;
using System.Data;
using System.Data.SqlClient;

namespace MovtechProject.Services
{
    public class QuestionsService
    {
        private readonly Database _database;
        public QuestionsService(Database database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<List<Questions>> GetQuestionsAsync()
        {
            List<Questions> Questions = new List<Questions>();

            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM perguntas", connection);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Questions.Add(new Questions
                            {
                                Id = (int)reader["id"],
                                Text = reader.GetString("texto"),
                                IdForms = (int)reader["idFormulario"]
                            });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao obter as perguntas: ", ex.Message);
                throw;
            }

            return Questions;
        }

        public async Task<Questions> GetQuestionsByIdAsync(int id)
        {
            Questions? Questions = null;
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM perguntas WHERE @id = id", connection);
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if( await reader.ReadAsync())
                        {
                            Questions = new Questions
                            {
                                Id = (int)reader["id"],
                                Text = reader["texto"].ToString(),
                                IdForms = (int)reader["idFormulario"]
                            };
                        }
                    }

                }
            }catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao obter a pergunta por ID:", ex.Message);
                throw;
            }

            return Questions;
        }

        public async Task<int> CreateQuestionsAsync(Questions questions)
        {
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("INSERT INTO perguntas (texto, idFormulario) VALUES (@texto, @idFormulario); SELECT SCOPE_IDENTITY();", connection);
                    command.Parameters.AddWithValue("@texto", questions.Text);
                    command.Parameters.AddWithValue("@idFormulario", questions.IdForms);

                    var insertedId = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(insertedId);
                }
            }catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao criar uma pergunta:", ex.Message);
                throw;
            }
        }

        public async Task<bool> UpdateQuestionsAsync(int id, Questions questions)
        {
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("UPDATE perguntas SET texto = @texto, idFormulario = @idFormulario WHERE id = @id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@texto", questions.Text);
                    command.Parameters.AddWithValue("@idFormulario", questions.IdForms);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao atualizar a pergunta:", ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteQuestionsAsync(int id)
        {
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("DELETE FROM perguntas WHERE @id = id", connection);
                    command.Parameters.AddWithValue("@id", id);

                    int rowAffected = await command.ExecuteNonQueryAsync();
                    return rowAffected > 0;
                }
            }catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao excluir a pergunta:", ex.Message);
                throw;
            }
        }
    }
}
