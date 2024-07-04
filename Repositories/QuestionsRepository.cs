using MovtechProject.Data;
using MovtechProject.Models;
using System.Data;
using System.Data.SqlClient;

namespace MovtechProject.Repositories
{
    public class QuestionsRepository
    {
        private readonly Database _database;

        public QuestionsRepository(Database database)
        {
            _database = database;
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
                                IdForms = (int)reader["idFormulario"],
                                Answers = new List<Answers>()
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

        public async Task<Questions?> GetQuestionsByIdAsync(int id)
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
                        if (await reader.ReadAsync())
                        {
                            Questions = new Questions
                            {
                                Id = (int)reader["id"],
                                Text = reader.GetString("texto"),
                                IdForms = (int)reader["idFormulario"],
                                Answers = new List<Answers>()
                            };
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao obter a pergunta por ID:", ex.Message);
                throw;
            }

            return Questions;
        }

        public async Task<Questions> CreateQuestionsAsync(Questions questions, int formId)
        {
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("INSERT INTO perguntas (texto, idFormulario) VALUES (@texto, @idFormulario); SELECT SCOPE_IDENTITY();", connection);
                    command.Parameters.AddWithValue("@texto", questions.Text);
                    command.Parameters.AddWithValue("@idFormulario", formId);

                    var insertedId = await command.ExecuteScalarAsync();
                    questions.Id = Convert.ToInt32(insertedId);
                    questions.IdForms = formId;

                    return questions;
                }
            }
            catch (Exception ex)
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
            }
            catch (Exception ex)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao excluir a pergunta:", ex.Message);
                throw;
            }
        }


        public async Task<List<Questions>> GetByFormsId(int idForms)
        {
            List<Questions> Question = new List<Questions>();

            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM perguntas WHERE @idFormulario = idFormulario", connection);
                    command.Parameters.AddWithValue("@idFormulario", idForms);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Question.Add(new Questions
                            {
                                Id = (int)reader["id"],
                                Text = reader.GetString("texto"),
                                IdForms = (int)reader["idFormulario"],
                                Answers = new List<Answers>()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao obter o id de formularios: ", ex.Message);
                throw;
            }


            return Question;
        }
    }
}
