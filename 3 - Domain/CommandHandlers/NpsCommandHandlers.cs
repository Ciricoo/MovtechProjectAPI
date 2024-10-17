using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;
using MovtechProject.Services;

namespace MovtechProject._3___Domain.CommandHandlers
{
    public class NpsCommandHandlers
    {

        private readonly AnswerRepository _answerRepository;
        private readonly FormGroupCommandHandlers _formGroupCommandHandlers;

        public NpsCommandHandlers(AnswerRepository answerRepository, FormGroupCommandHandlers formGroupCommandHandlers)
        {
            _answerRepository = answerRepository;
            _formGroupCommandHandlers = formGroupCommandHandlers;
        }

        public int NpsCalculator(List<Answer> answers)
        {
            int totalAnswers = answers.Count;
            List<Answer> promoters = answers.Where(a => a.Grade >= 9).ToList();
            List<Answer> detractors = answers.Where(a => a.Grade <= 6).ToList();

            double promotersPercentage = (promoters.Count / (double)totalAnswers) * 100;
            double detractorsPercentage = (detractors.Count / (double)totalAnswers) * 100;

            int npsScore = (int)(promotersPercentage - detractorsPercentage);

            return npsScore;
        }

        public async Task<int> NpsScore()
        {
            List<Answer> answers = await _answerRepository.GetAnswersAsync();

            if (!answers.Any())
            {
                return 0;
            }

           return NpsCalculator(answers);

        }

        public async Task<List<int>> GetAnswersAccordingNpsGrade()
        {
            List<Answer> answers = await _answerRepository.GetAnswersAsync();
            int promoters = answers.Where(x => x.Grade >= 9 && x.Grade <= 10).Count();
            int passives = answers.Where(x => x.Grade >= 7 && x.Grade <= 8).Count();
            int detractors = answers.Where(x => x.Grade >= 0 && x.Grade <= 6).Count();
            List<int> list = new List<int> { promoters, passives, detractors };
            return list;
        }

        public async Task<int> GetNpsByGroupId(int idGroup)
        {
            FormGroup group = await _formGroupCommandHandlers.GetFormsGroupByIdAsync(idGroup);

            if (group == null)
            {
                throw new KeyNotFoundException($"ID {idGroup} não encontrado!");
            }

            List<Question> questions = group.Forms.SelectMany(f => f.Questions).ToList();

            List<Answer> answers = new List<Answer>();

            foreach (Question question in questions)
            {
                answers.AddRange(await _answerRepository.GetAnswersByQuestionId(question.Id));
            }

            return NpsCalculator(answers);

        }
    }
}
