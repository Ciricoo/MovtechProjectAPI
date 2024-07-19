using MovtechProject.DataAcess.Data;
using MovtechProject.Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace MovtechProject.DataAcess.Repositories
{
    public class QuestionRepository
    {
        private readonly Database _database;

        public QuestionRepository(Database database)
        {
            _database = database;
        }

        public async Task<List<Question>> GetQuestionsAsync()
        {
            List<Question> Questions = new List<Question>();

            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM perguntas", connection);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Questions.Add(new Question
                        {
                            Id = (int)reader["id"],
                            Text = reader.GetString("texto"),
                            IdForms = (int)reader["idFormulario"],
                        });
                    }
                }
            }

            return Questions;
        }

        public async Task<Question?> GetQuestionsByIdAsync(int id)
        {
            Question? Questions = null;

            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM perguntas WHERE @id = id", connection);
                command.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        Questions = new Question
                        {
                            Id = (int)reader["id"],
                            Text = reader.GetString("texto"),
                            IdForms = (int)reader["idFormulario"],
                        };
                    }
                }

            }

            return Questions;
        }

        public async Task<Question> CreateQuestionsAsync(Question questions)
        {
            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("INSERT INTO perguntas (texto, idFormulario) VALUES (@texto, @idFormulario); SELECT SCOPE_IDENTITY();", connection);
                command.Parameters.AddWithValue("@texto", questions.Text);
                command.Parameters.AddWithValue("@idFormulario", questions.IdForms);

                var insertedId = await command.ExecuteScalarAsync();
                questions.Id = Convert.ToInt32(insertedId);

                return questions;
            }
        }

        public async Task<bool> UpdateQuestionsAsync(int id, Question questions)
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

        public async Task<bool> DeleteQuestionsAsync(int id)
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


        public async Task<List<Question>> GetQuestionByFormsId(int idForms)
        {
            List<Question> Question = new List<Question>();

            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM perguntas WHERE @idFormulario = idFormulario", connection);
                command.Parameters.AddWithValue("@idFormulario", idForms);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Question.Add(new Question
                        {
                            Id = (int)reader["id"],
                            Text = reader.GetString("texto"),
                            IdForms = (int)reader["idFormulario"],
                        });
                    }
                }
            }

            return Question;
        }
    }
}
