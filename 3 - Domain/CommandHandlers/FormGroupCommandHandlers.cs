using Microsoft.SqlServer.Server;
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

            foreach (Form form in formsGroup.Forms)
            {
                if (string.IsNullOrWhiteSpace(form.Name))
                {
                    throw new ArgumentException("O texto do formulário não pode ser vazio!");
                }

                if (form.Questions.Where(x => x.Text.Trim() == "").Any())
                {
                    throw new ArgumentException("O texto da pergunta não pode ser vazio!");
                }
            }

            FormGroup createdGroup = await _formsGroupRepository.CreateFormsGroupAsync(formsGroup);

            foreach (Form form in formsGroup.Forms)
            {
                form.IdFormsGroup = createdGroup.Id;
                await _formsRepository.CreateFormsAsync(form);

                foreach (Question question in form.Questions)
                {
                    question.IdForms = form.Id;
                }

                await _questionsRepository.CreateQuestionsAsync(form.Questions.ToList());
            }

            return createdGroup;
        }

        public async Task<bool> UpdateFormsGroupAsync(int id, FormGroup formsGroup)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(formsGroup.Name))
            {
                throw new ArgumentException("O nome do grupo de formulários é inválido!", formsGroup.Name);
            }

            FormGroup? formsGroupUpdateId = await _formsGroupRepository.GetFormsGroupByIdAsync(id);

            if (formsGroupUpdateId == null)
            {
                throw new KeyNotFoundException($"ID {id} não encontrado!");
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

            List<int> formsIds = await _formsRepository.GetFormsIdsByGroupId(id);

            if (formsIds.Any())
            {
                List<int> questionsIds = await _questionsRepository.GetQuestionIdsByFormsIds(formsIds);

                if (questionsIds.Any())
                {
                    await _answerRepository.DeleteAnswersByQuestionIds(questionsIds);

                    await _questionsRepository.DeleteQuestionsIdsAsync(questionsIds);

                }

                await _formsRepository.DeleteFormsAsync(formsIds);
            }

            return await _formsGroupRepository.DeleteFormsGroupAsync(id);
        }
    }
}
