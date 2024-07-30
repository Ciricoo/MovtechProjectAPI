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

        public async Task<List<Answer>> CreateAnswersAsync(List<Answer> answers)
        {
            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();

                DataTable dt = new DataTable();
                dt.Columns.Add("nota", typeof(int));
                dt.Columns.Add("descricao", typeof(string));
                dt.Columns.Add("idPerguntas", typeof(int));
                dt.Columns.Add("idUsuario", typeof(int));

                foreach (var answer in answers)
                {
                    dt.Rows.Add(answer.Grade, answer.Description, answer.IdQuestion, answer.IdUser);
                }

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "respostas";
                    bulkCopy.ColumnMappings.Add("nota", "nota");
                    bulkCopy.ColumnMappings.Add("descricao", "descricao");
                    bulkCopy.ColumnMappings.Add("idPerguntas", "idPerguntas");
                    bulkCopy.ColumnMappings.Add("idUsuario", "idUsuario");

                    await bulkCopy.WriteToServerAsync(dt);
                }

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

        public async Task<List<Answer>> GetAnswerByUserIdAsync(int idUser)
        {
            List<Answer> answerUser = new List<Answer>();

            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT * FROM respostas WHERE idUsuario = @idUsuario", connection);
                command.Parameters.AddWithValue("@idUsuario", idUser);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        answerUser.Add(new Answer()
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
            return answerUser;
        }
    }
}

