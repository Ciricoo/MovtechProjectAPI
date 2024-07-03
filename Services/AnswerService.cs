using MovtechProject.Models;
using MovtechProject.Repositories;

namespace MovtechProject.Services
{
    public class AnswerService
    {
        private readonly AnswerRepository _answerRepository;

        public AnswerService(AnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<List<Answers>> GetAnswersAsync()
        {
            var list = await _answerRepository.GetAnswersAsync();

            if (list == null)
            {
                throw new ArgumentException("Não existe nenhuma resposta!");
            }

            return list;
        }

        public async Task<Answers> GetAnswersByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            Answers? answers = await _answerRepository.GetAnswersByIdAsync(id);

            if (answers == null)
            {
                throw new ArgumentException("Id não encontrado!");
            }

            return answers;
        }

        public async Task<Answers> CreateAnswersAsync(Answers answers)
        {
            if (answers.Note <= 0 || answers.Note > 10)
            {
                throw new ArgumentException("Nota inválida!");
            }

            return await _answerRepository.CreateAnswersAsync(answers);
        }
    }
}
