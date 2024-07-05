using MovtechProject.Data;
using MovtechProject.Models;
using System.Data;
using System.Data.SqlClient;

namespace MovtechProject.Repositories
{
    public class AnswerRepository
    {
        private readonly Database _database;

        public AnswerRepository(Database database)
        {
            _database = database;
        }

        public async Task<List<Answers>> GetAnswersAsync()
        {
            List<Answers> answerList = new List<Answers>();

            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM respostas", connection);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            answerList.Add(new Answers()
                            {
                                Id = (int)reader["id"],
                                Note = (int)reader["nota"],
                                Description = reader.GetString("descricao"),
                                IdQuestion = (int)reader["idPerguntas"],
                                IdUser = (int)reader["idUsuario"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao obter grupos de formulários: ", ex.Message);
                throw;
            }
            return answerList;
        }

        public async Task<Answers?> GetAnswersByIdAsync(int id)
        {
            Answers? answer = null;

            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM respostas WHERE id = @id", connection);
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            answer = (new Answers
                            {
                                Id = (int)reader["id"],
                                Note = (int)reader["nota"],
                                Description = reader.GetString("descricao"),
                                IdQuestion = (int)reader["idPerguntas"],
                                IdUser = (int)reader["idUsuario"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao obter a resposta por ID:", ex.Message);
                throw;
            }

            return answer;
        }

        public async Task<Answers> CreateAnswersAsync(Answers answers)
        {
            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("INSERT INTO respostas (nota, descricao, idPerguntas, idUsuario) " +
                        "VALUES (@nota, @descricao, @idPerguntas, @idUsuario) SELECT SCOPE_IDENTITY()", connection);
                    command.Parameters.AddWithValue("@nota", answers.Note);
                    command.Parameters.AddWithValue("@descricao", answers.Description);
                    command.Parameters.AddWithValue("@idPerguntas", answers.IdQuestion);
                    command.Parameters.AddWithValue("@idUsuario", answers.IdUser);

                    var insertedId = await command.ExecuteNonQueryAsync();
                    answers.Id = insertedId;
                    return answers;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro não esperado ao criar a pergunta:", ex.Message);
                throw;
            }
        }

        public async Task<List<Answers>> GetAnswerByQuestionId(int idQuestion)
        {
            List<Answers> Answer = new List<Answers>();

            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM respostas WHERE @idPerguntas = idPerguntas", connection);
                    command.Parameters.AddWithValue("@idPerguntas", idQuestion);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Answer.Add(new Answers
                            {
                                Id = (int)reader["id"],
                                Note = (int)reader["nota"],
                                Description = reader.GetString("descricao"),
                                IdQuestion = (int)reader["idPerguntas"],
                                IdUser = (int)reader["idUsuario"]
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
            return Answer;
        }

        public async Task<bool> DeleteAnswerByQuestionId(int questionId)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao excluir perguntas por ID do formulário:", ex.Message);
                throw;
            }
        }
    }
}

