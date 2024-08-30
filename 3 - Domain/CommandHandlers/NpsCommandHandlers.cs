using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;

namespace MovtechProject._3___Domain.CommandHandlers
{
    public class NpsCommandHandlers
    {

        private readonly AnswerRepository _answerRepository;

        public NpsCommandHandlers(AnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<int> NpsScore()
        {
            List<Answer> answers = await _answerRepository.GetAnswersAsync();

            if (!answers.Any())
            {
                return 0;
            }

            int totalAnswers = answers.Count;
            List<Answer> promoters = answers.Where(a => a.Grade >= 9).ToList();
            List<Answer> detractors = answers.Where(a => a.Grade <= 6).ToList();

            double promotersPercentage = (promoters.Count / (double)totalAnswers) * 100;
            double detractorsPercentage = (detractors.Count / (double)totalAnswers) * 100;

            int npsScore = (int)(promotersPercentage - detractorsPercentage);

            return npsScore;

        }
    }
}
