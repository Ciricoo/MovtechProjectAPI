using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;
using MovtechProject.Services;

namespace MovtechProject._3___Domain.CommandHandlers
{
    public class QuestionCommandHandlers
    {
        private readonly FormRepository _formRepository;
        private readonly QuestionRepository _questionsRepository;
        private readonly AnswerRepository _answerRepository;
        public QuestionCommandHandlers(FormRepository formRepository, QuestionRepository questionsRepository, AnswerRepository answerRepository)
        {
            _formRepository = formRepository;
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
                throw new KeyNotFoundException("Id não encontrado!");
            }

            return questions;
        }

        public async Task<Question> CreateQuestionsAsync(Question questions)
        {
            if (string.IsNullOrWhiteSpace(questions.Text))
            {
                throw new ArgumentException("O texto da pergunta não pode ser vazio!");
            }

            Form? FkForm = await _formRepository.GetFormsByIdAsync(questions.Id);

            if(FkForm == null)
            {
                throw new KeyNotFoundException("FK do formulário inválida!");
            }

            return await _questionsRepository.CreateQuestionsAsync(questions);
        }

        public async Task<bool> UpdateQuestionsAsync(int id, Question questions)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            Question? updateQuestion = await _questionsRepository.GetQuestionsByIdAsync(id);

            if (updateQuestion == null)
            {
                throw new KeyNotFoundException("Id não encontrado!");
            }

            Form? FkForm = await _formRepository.GetFormsByIdAsync(questions.Id);

            if (FkForm == null)
            {
                throw new KeyNotFoundException("FK do formulário inválida!");
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

            Question? deleteQuestion = await _questionsRepository.GetQuestionsByIdAsync(id);

            if (deleteQuestion == null)
            {
                throw new KeyNotFoundException("Id não encontrado!");
            }

            await _answerRepository.DeleteAnswerByQuestionId(id);

            return await _questionsRepository.DeleteQuestionsAsync(id);
        }

    }
}
