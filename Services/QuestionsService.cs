using MovtechProject.Data;
using MovtechProject.Models;
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
                                Text = reader["texto"].ToString(),
                                IdForms = (int)reader["idFormularios"]
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
            Questions Questions = null;

            try
            {
                using (SqlConnection connection = _database.GetConnection())
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("SELECT * FROM perguntas WHERE @id = id", connection);
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if( await  reader.ReadAsync())
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
    }
}
