using Microsoft.SqlServer.Server;
using MovtechProject.Data;
using MovtechProject.Models;
using MovtechProject.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace MovtechProject.Services
{
    public class QuestionsService
    {
        private readonly QuestionsRepository _questionsRepository;
        public QuestionsService(QuestionsRepository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        public async Task<List<Questions>> GetQuestionsAsync()
        {
            var lista = _questionsRepository.GetQuestionsAsync();

            if (lista == null)
            {
                throw new ArgumentException("Não existe nenhuma pergunta!");
            }

            return await _questionsRepository.GetQuestionsAsync();
        }

        public async Task<Questions> GetQuestionsByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            Questions questions = await _questionsRepository.GetQuestionsByIdAsync(id);

            if (questions == null)
            {
                throw new ArgumentException("Id não encontrado!");
            }

            return await _questionsRepository.GetQuestionsByIdAsync(id);
        }

        public async Task<Questions> CreateQuestionsAsync(Questions questions)
        {
            if (string.IsNullOrWhiteSpace(questions.Text))
            {
                throw new ArgumentException("O texto da pergunta é inválido!");
            }

            return await _questionsRepository.CreateQuestionsAsync(questions);
        }

        public async Task<bool> UpdateQuestionsAsync(int id, Questions questions)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            Questions existingQuestions = await _questionsRepository.GetQuestionsByIdAsync(id);

            if (existingQuestions == null)
            {
                throw new InvalidOperationException($"Pergunta com ID {id} não encontrado!");
            }

            if (string.IsNullOrWhiteSpace(questions.Text) || questions.Text.Length > 100)
            {
                throw new ArgumentException("O texto da pergunta é inválido!");
            }

            return await _questionsRepository.UpdateQuestionsAsync(id, questions);
        }

        public async Task<bool> DeleteQuestionsAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            Questions existingQuestions = await _questionsRepository.GetQuestionsByIdAsync(id);

            if (existingQuestions == null)
            {
                throw new InvalidOperationException($"Pergunta com ID {id} não encontrado!");
            }

            return await _questionsRepository.DeleteQuestionsAsync(id);
        }
    }
}
