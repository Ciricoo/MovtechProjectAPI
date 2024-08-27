using MovtechProject._3___Domain.CommandHandlers;
using MovtechProject.Domain.Models;

namespace MovtechProject.Services
{
    public class QuestionService
    {
        private readonly QuestionCommandHandlers _questionCommandHandlers;

        public QuestionService(QuestionCommandHandlers questionCommandHandlers)
        {
            _questionCommandHandlers = questionCommandHandlers;
        }

        public async Task<List<Question>> GetQuestionsAsync()
        {
            return await _questionCommandHandlers.GetQuestionsAsync();
        }

        public async Task<Question> GetQuestionsByIdAsync(int id)
        {
            return await _questionCommandHandlers.GetQuestionsByIdAsync(id);
        }

        public async Task<List<Question>> CreateQuestionsAsync(List<Question> questions)
        {
            return await _questionCommandHandlers.CreateQuestionsAsync(questions);
        }

        public async Task<bool> UpdateQuestionsAsync(int id, Question questions)
        {
            return await _questionCommandHandlers.UpdateQuestionsAsync(id, questions);
        }

        public async Task<bool> DeleteQuestionsAsync(int id)
        {
            return await _questionCommandHandlers.DeleteQuestionsAsync(id);
        }

        public async Task<List<Question>> GetQuestionByFormsId(int idForms)
        {
            return await _questionCommandHandlers.GetQuestionByFormsId(idForms);
        }
    }
}
