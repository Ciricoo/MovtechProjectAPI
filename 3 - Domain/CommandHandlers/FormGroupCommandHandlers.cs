using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;

namespace MovtechProject._3___Domain.CommandHandlers
{
    public class FormGroupCommandHandlers
    {
        private readonly FormGroupRepository _formsGroupRepository;
        private readonly FormRepository _formsRepository;
        private readonly QuestionRepository _questionsRepository;
        private readonly AnswerRepository _answerRepository;

        public FormGroupCommandHandlers(FormGroupRepository formsGroupRepository, FormRepository formsRepository, QuestionRepository questionsRepository, AnswerRepository answerRepository)
        {
            _formsGroupRepository = formsGroupRepository;
            _formsRepository = formsRepository;
            _questionsRepository = questionsRepository;
            _answerRepository = answerRepository;
        }

        public async Task<List<FormGroup>> GetFormsGroupsAsync()
        {
            List<FormGroup> list = await _formsGroupRepository.GetFormsGroupsAsync();

            if (!list.Any())
            {
                throw new ArgumentNullException("Não existe grupo de formulários!");
            }

            return list;
        }

        public async Task<FormGroup> GetFormsGroupByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!", nameof(id));
            }

            FormGroup? formsGroup = await _formsGroupRepository.GetFormsGroupByIdAsync(id);

            if (formsGroup == null)
            {
                throw new KeyNotFoundException($"ID {id} não encontrado!");
            }

            formsGroup.Forms = await _formsRepository.GetFormsByGroupId(id);

            foreach (Form form in formsGroup.Forms)
            {
                form.Questions = await _questionsRepository.GetQuestionByFormsId(form.Id);
            }

            return formsGroup;
        }

        public async Task<FormGroup> CreateFormsGroupAsync(FormGroup formsGroup)
        {
            if (string.IsNullOrWhiteSpace(formsGroup.Name))
            {
                throw new ArgumentException("O nome do grupo é inválido!", formsGroup.Name);
            }

            return await _formsGroupRepository.CreateFormsGroupAsync(formsGroup);
        }

        public async Task<bool> UpdateFormsGroupAsync(int id, FormGroup formsGroup)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!", nameof(id));
            }

            FormGroup? formsGroupUpdateId = await _formsGroupRepository.GetFormsGroupByIdAsync(id);

            if (formsGroupUpdateId == null)
            {
                throw new KeyNotFoundException($"ID {id} não encontrado!");
            }

            if (string.IsNullOrWhiteSpace(formsGroup.Name))
            {
                throw new ArgumentException("O nome do grupo de formulários é inválido!", formsGroup.Name);
            }

            return await _formsGroupRepository.UpdateFormsGroupAsync(id, formsGroup);
        }

        public async Task<bool> DeleteFormsGroupAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!", nameof(id));
            }

            FormGroup? deletedFormGroup = await _formsGroupRepository.GetFormsGroupByIdAsync(id);

            if (deletedFormGroup == null)
            {
                throw new KeyNotFoundException($"Id {id} não encontrado!");
            }

            List<Form> forms = await _formsRepository.GetFormsByGroupId(id);

            foreach (Form form in forms)
            {
                List<Question> questions = await _questionsRepository.GetQuestionByFormsId(form.Id);

                foreach (Question question in questions)
                {
                    await _answerRepository.DeleteAnswerByQuestionId(question.Id);
                    await _questionsRepository.DeleteQuestionsAsync(question.Id);
                }

                await _formsRepository.DeleteFormsAsync(form.Id);
            }

            return await _formsGroupRepository.DeleteFormsGroupAsync(id);
        }
    }
}

