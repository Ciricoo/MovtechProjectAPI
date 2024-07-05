using MovtechProject.Models;
using MovtechProject.Repositories;

namespace MovtechProject.Services
{
    public class FormsGroupService
    {
        private readonly FormsGroupRepository _formsGroupRepository;
        private readonly FormsRepository _formsRepository;
        private readonly QuestionsRepository _questionsRepository;
        private readonly AnswerRepository _answerRepository;

        public FormsGroupService(FormsGroupRepository formsGroupRepository, FormsRepository formsRepository, QuestionsRepository questionsRepository, AnswerRepository answerRepository)
        {
            _formsGroupRepository = formsGroupRepository;
            _formsRepository = formsRepository;
            _questionsRepository = questionsRepository;
            _answerRepository = answerRepository;
        }

        public async Task<List<FormsGroup>> GetFormsGroupsAsync()
        {
            var list = await _formsGroupRepository.GetFormsGroupsAsync();

            if(list == null || list.Count == 0)
            {
                throw new ArgumentException("Não existe grupo de formulários!");
            }

            return list;
        }

        public async Task<FormsGroup> GetFormsGroupByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            FormsGroup? formsGroup = await _formsGroupRepository.GetFormsGroupByIdAsync(id);

            if (formsGroup == null)
            {
                throw new ArgumentException("ID não encontrado!");
            }

            formsGroup.Forms = await _formsRepository.GetFormsByGroupId(id);

            foreach (Forms form in formsGroup.Forms)
            {
                form.Questions = await _questionsRepository.GetQuestionByFormsId(form.Id);

                foreach (Questions question in form.Questions)
                {
                    question.Answers = await _answerRepository.GetAnswerByQuestionId(question.Id);
                }
            }

            return formsGroup;
        }

        public async Task<FormsGroup> CreateFormsGroupAsync(FormsGroup formsGroup)
        {
            if (string.IsNullOrWhiteSpace(formsGroup.Name) || formsGroup.Name.Length > 100)
            {
                throw new ArgumentException("O nome do grupo é inválido!");
            }

            return await _formsGroupRepository.CreateFormsGroupAsync(formsGroup);
        }

        public async Task<bool> UpdateFormsGroupAsync(int id, FormsGroup formsGroup)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            if (string.IsNullOrWhiteSpace(formsGroup.Name) || formsGroup.Name.Length > 100)
            {
                throw new ArgumentException("O nome do grupo de formulários é inválido!");
            }

            return await _formsGroupRepository.UpdateFormsGroupAsync(id, formsGroup);
        }

        public async Task<bool> DeleteFormsGroupAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            List<Forms> forms = await _formsRepository.GetFormsByGroupId(id);

            foreach (Forms form in forms)
            {
                List<Questions> questions = await _questionsRepository.GetQuestionByFormsId(form.Id);

                foreach (Questions question in questions)
                {
                    await _answerRepository.DeleteAnswerByQuestionId(question.Id);
                }

                await _questionsRepository.DeleteQuestionByFormsId(form.Id);
                await _formsRepository.DeleteFormsAsync(form.Id);
            }

            bool deleted = await _formsGroupRepository.DeleteFormsGroupAsync(id);

            if (deleted == false)
            {
                throw new ArgumentException("Id não encontrado!");
            }

            return deleted;
        }
    }
}
