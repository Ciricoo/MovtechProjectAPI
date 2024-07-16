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

        public async Task<Answer> CreateAnswersAsync(Answer answers)
        {
            return await _answerCommandHandlers.CreateAnswersAsync(answers);
        }
    }
}
