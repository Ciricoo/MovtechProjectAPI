using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;

namespace MovtechProject.Services
{
    public class FormService
    {
        private readonly FormRepository _formsRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly AnswerRepository _answerRepository;

        public FormService(FormRepository formsRepository, QuestionRepository questionRepository, AnswerRepository answerRepository)
        {
            _formsRepository = formsRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        
        }
        public async Task<List<Form>> GetFormsAsync()
        {
            List<Form> list = await _formsRepository.GetFormsAsync();

            if (list == null)
            {
                throw new ArgumentException("Não existe nenhum formulário!");
            }

            return list;
        }

        public async Task<Form> GetFormsByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            Form? forms = await _formsRepository.GetFormsByIdAsync(id);

            if (forms == null)
            {
                throw new ArgumentException("Id não encontrado!");
            }

            forms.Questions = await _questionRepository.GetQuestionByFormsId(id);

            return forms;
        }

        public async Task<Form> CreateFormsAsync(Form forms)
        {
            if (string.IsNullOrWhiteSpace(forms.Name))
            {
                throw new ArgumentException("O nome do formulário é inválido!");
            }

            Form createdForms = await _formsRepository.CreateFormsAsync(forms);

            foreach (Question question in forms.Questions)
            {
                question.IdForms = createdForms.Id;
                await _questionRepository.CreateQuestionsAsync(question);
            }
            return createdForms;
        }

        public async Task<bool> UpdateFormsAsync(int id, Form forms)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            if (string.IsNullOrWhiteSpace(forms.Name))
            {
                throw new ArgumentException("O nome do formulário é inválido!");
            }

            return await _formsRepository.UpdateFormsAsync(id, forms);
        }

        public async Task<bool> DeleteFormsAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            List<Question> questions = await _questionRepository.GetQuestionByFormsId(id);

            foreach (Question question in questions)
            {
                await _answerRepository.DeleteAnswerByQuestionId(question.Id);
            }
            await _questionRepository.DeleteQuestionByFormsId(id);

            bool deleted = await _formsRepository.DeleteFormsAsync(id);

            if (deleted == false)
            {
                throw new ArgumentException("Id não encontrado!");
            }

            return deleted;
        }
    }
}

