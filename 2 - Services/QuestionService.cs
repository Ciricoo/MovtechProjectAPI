using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;

namespace MovtechProject.Services
{
    public class QuestionService
    {
        private readonly QuestionRepository _questionsRepository;
        private readonly AnswerRepository _answerRepository;
        public QuestionService(QuestionRepository questionsRepository, AnswerRepository answerRepository)
        {
            _questionsRepository = questionsRepository;
            _answerRepository = answerRepository;
        }

        public async Task<List<Question>> GetQuestionsAsync()
        {
            List<Question> list = await _questionsRepository.GetQuestionsAsync();

            if (list == null)
            {
                throw new ArgumentException("Não existe nenhuma pergunta!");
            }

            return list;
        }

        public async Task<Question> GetQuestionsByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            Question? questions = await _questionsRepository.GetQuestionsByIdAsync(id);

            if (questions == null)
            {
                throw new ArgumentException("Id não encontrado!");
            }

            return questions;
        }

        public async Task<Question> CreateQuestionsAsync(Question questions)
        {
            if (string.IsNullOrWhiteSpace(questions.Text))
            {
                throw new ArgumentException("O texto da pergunta não pode ser vazio!");
            }

            return await _questionsRepository.CreateQuestionsAsync(questions);
        }

        public async Task<bool> UpdateQuestionsAsync(int id, Question questions)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            if (string.IsNullOrWhiteSpace(questions.Text))
            {
                throw new ArgumentException("O texto da pergunta não pode ser vazio!");
            }

            return await _questionsRepository.UpdateQuestionsAsync(id, questions);
        }

        public async Task<bool> DeleteQuestionsAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            await _answerRepository.DeleteAnswerByQuestionId(id);

            bool deleted = await _questionsRepository.DeleteQuestionsAsync(id);

            if (deleted == false)
            {
                throw new ArgumentException("Id não encontrado!");
            }

            return deleted;
        }
    }
}
