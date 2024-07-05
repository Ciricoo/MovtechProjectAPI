using MovtechProject.Models;
using MovtechProject.Repositories;

namespace MovtechProject.Services
{
    public class FormsService
    {
        private readonly FormsRepository _formsRepository;
        private readonly QuestionsRepository _questionRepository;
        private readonly AnswerRepository _answerRepository;

        public FormsService(FormsRepository formsRepository, QuestionsRepository questionRepository, AnswerRepository answerRepository)
        {
            _formsRepository = formsRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }


        public async Task<List<Forms>> GetFormsAsync()
        {
            var list = await _formsRepository.GetFormsAsync();

            if (list == null)
            {
                throw new ArgumentException("Não existe nenhum formulário!");
            }

            return list;
        }

        public async Task<Forms> GetFormsByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            Forms? forms = await _formsRepository.GetFormsByIdAsync(id);

            if (forms == null)
            {
                throw new ArgumentException("Id não encontrado!");
            }

            forms.Questions = await _questionRepository.GetQuestionByFormsId(id);

            foreach (Questions question in forms.Questions)
            {
                question.Answers = await _answerRepository.GetAnswerByQuestionId(question.Id);
            }
            return forms;
        }

        public async Task<Forms> CreateFormsAsync(Forms forms)
        {
            if (string.IsNullOrWhiteSpace(forms.Name) || forms.Name.Length > 100)
            {
                throw new ArgumentException("O nome do formulário é inválido!");
            }

            Forms createdForms = await _formsRepository.CreateFormsAsync(forms);

            Console.WriteLine(createdForms);

            foreach (Questions question in createdForms.Questions)
            {
                question.IdForms = createdForms.Id;
                createdForms.Questions.Add(await _questionRepository.CreateQuestionsAsync(question));
            }
            return createdForms;
        }

        public async Task<bool> UpdateFormsAsync(int id, Forms forms)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            if (string.IsNullOrWhiteSpace(forms.Name) || forms.Name.Length > 100)
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

            List<Questions> questions = await _questionRepository.GetQuestionByFormsId(id);

            foreach (Questions question in questions)
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

