using MovtechProject.DataAcess.Data;
using MovtechProject.Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace MovtechProject.DataAcess.Repositories
{
    public class AnswerRepository
    {
        private readonly Database _database;

        public AnswerRepository(Database database)
        {
            _database = database;
        }

        public async Task<List<Answer>> GetAnswersAsync()
        {
            List<Answer> answerList = new List<Answer>();

            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM respostas", connection);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        answerList.Add(new Answer()
                        {
                            Id = (int)reader["id"],
                            Grade = (int)reader["nota"],
                            Description = reader.GetString("descricao"),
                            IdQuestion = (int)reader["idPerguntas"],
                            IdUser = (int)reader["idUsuario"]
                        });
                    }
                }
            }

            return answerList;
        }

        public async Task<Answer?> GetAnswersByIdAsync(int id)
        {
            Answer? answer = null;

            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM respostas WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        answer = new Answer
                        {
                            Id = (int)reader["id"],
                            Grade = (int)reader["nota"],
                            Description = reader.GetString("descricao"),
                            IdQuestion = (int)reader["idPerguntas"],
                            IdUser = (int)reader["idUsuario"]
                        };
                    }
                }
            }

            return answer;
        }

        public async Task<Answer> CreateAnswersAsync(Answer answers)
        {
            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("INSERT INTO respostas (nota, descricao, idPerguntas, idUsuario) " +
                    "VALUES (@nota, @descricao, @idPerguntas, @idUsuario) SELECT SCOPE_IDENTITY()", connection);
                command.Parameters.AddWithValue("@nota", answers.Grade);
                command.Parameters.AddWithValue("@descricao", answers.Description);
                command.Parameters.AddWithValue("@idPerguntas", answers.IdQuestion);
                command.Parameters.AddWithValue("@idUsuario", answers.IdUser);

                var insertedId = await command.ExecuteScalarAsync();
                answers.Id = Convert.ToInt32(insertedId);
                return answers;
            }
        }

        public async Task<bool> DeleteAnswerByQuestionId(int questionId)
        {
            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("DELETE FROM respostas WHERE idPerguntas = @idPerguntas", connection);
                command.Parameters.AddWithValue("@idPerguntas", questionId);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}

