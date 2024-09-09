﻿using MovtechProject.DataAcess.Data;
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
                SqlCommand command = new SqlCommand($"SELECT * FROM respostas WHERE id = {id}", connection);

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

                foreach (Answer answer in answers)
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
                SqlCommand command = new SqlCommand($"DELETE FROM respostas WHERE idPerguntas = {questionId}", connection);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAnswersByQuestionIds(List<int> questionIds)
        {
            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                string idsString = string.Join(", ", questionIds);
                SqlCommand command = new SqlCommand($"DELETE FROM respostas WHERE idPerguntas in ({idsString})", connection);

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
                SqlCommand command = new SqlCommand($"SELECT * FROM respostas WHERE idUsuario = {idUser}", connection);

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

        public async Task<List<Answer>> GetAnswersByQuestionId(int idQuestion)
        {
            List<Answer> answers = new List<Answer>();

            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand($"SELECT * FROM respostas WHERE idPerguntas = {idQuestion}", connection);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        answers.Add(new Answer
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
            return answers;
        }

        public async Task<List<Answer>> GetAnswersWithDetailsAsync(int? questionId = null, int? userId = null)
        {
            List<Answer> answers = new List<Answer>();

            using (SqlConnection connection = _database.GetConnection())
            {
                await connection.OpenAsync();

                string query = @"
                SELECT a.*, q.texto AS QuestionText, u.nome AS Username
                FROM respostas a
                JOIN perguntas q ON a.idPerguntas = q.id
                JOIN usuarios u ON a.idUsuario = u.id
                WHERE (@questionId IS NULL OR a.idPerguntas = @questionId)
                    AND (@userId IS NULL OR a.idUsuario = @userId)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@questionId", (object)questionId ?? DBNull.Value);
                command.Parameters.AddWithValue("@userId", (object)userId ?? DBNull.Value);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        answers.Add(new Answer
                        {
                            Id = (int)reader["id"],
                            IdQuestion = (int)reader["idPerguntas"],
                            IdUser = (int)reader["idUsuario"],
                            Grade = (int)reader["nota"],
                            Description = reader.GetString("descricao"),
                            QuestionText = reader.GetString("QuestionText"),
                            Username = reader.GetString("Username")
                        });
                    }
                }
            }

            return answers;
        }

    }
}

