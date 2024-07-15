using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;

namespace MovtechProject.Services
{
    public class AnswerService
    {
        private readonly AnswerRepository _answerRepository;

        public AnswerService(AnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<List<Answer>> GetAnswersAsync()
        {
            List<Answer> list = await _answerRepository.GetAnswersAsync();

            if (list == null)
            {
                throw new ArgumentException("Não existe nenhuma resposta!");
            }

            return list;
        }

        public async Task<Answer> GetAnswersByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            Answer? answers = await _answerRepository.GetAnswersByIdAsync(id);

            if (answers == null)
            {
                throw new ArgumentException("Id não encontrado!");
            }

            return answers;
        }

        public async Task<Answer> CreateAnswersAsync(Answer answers)
        {
            if (answers.Grade < 0 || answers.Grade > 10)
            {
                throw new ArgumentException("Nota inválida!");
            }

            return await _answerRepository.CreateAnswersAsync(answers);
        }
    }
}
