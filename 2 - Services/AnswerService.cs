using MovtechProject._3___Domain.CommandHandlers;
using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;

namespace MovtechProject.Services
{
    public class AnswerService
    {
        private readonly AnswerCommandHandlers _answerCommandHandlers;

        public AnswerService(AnswerCommandHandlers answerCommandHandlers)
        {
            _answerCommandHandlers = answerCommandHandlers;
        }

        public async Task<List<Answer>> GetAnswersAsync()
        {
            return await _answerCommandHandlers.GetAnswersAsync();
        }

        public async Task<Answer> GetAnswersByIdAsync(int id)
        {
            return await _answerCommandHandlers.GetAnswersByIdAsync(id);
        }

        public async Task<List<Answer>> CreateAnswersAsync(List<Answer> answers, HttpContext context)
        {
            return await _answerCommandHandlers.CreateAnswersAsync(answers, context);
        }

        public async Task<List<Answer>> GetAnswerByUserIdAsync(int idUser)
        {
            return await _answerCommandHandlers.GetAnswerByUserIdAsync(idUser);
        }

        public async Task<List<Answer>> GetAnswersByQuestionId(int idQuestion)
        {
            return await _answerCommandHandlers.GetAnswersByQuestionId(idQuestion);
        }

        public async Task<List<Answer>> GetAnswersWithDetailsAsync(int? questionId = null, int? userId = null)
        {
            return await _answerCommandHandlers.GetAnswersWithDetailsAsync(questionId, userId);
        }

        
    }
}
