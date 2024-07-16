﻿using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;

namespace MovtechProject._3___Domain.CommandHandlers
{
    public class FormCommandHandlers
    {
        private readonly FormGroupRepository _formGroupRepository;
        private readonly FormRepository _formsRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly AnswerRepository _answerRepository;

        public FormCommandHandlers(FormGroupRepository formGroupRepository, FormRepository formsRepository, QuestionRepository questionRepository, AnswerRepository answerRepository)
        {
            _formGroupRepository = formGroupRepository;
            _formsRepository = formsRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;

        }
        public async Task<List<Form>> GetFormsAsync()
        {
            List<Form> list = await _formsRepository.GetFormsAsync();

            if (!list.Any())
            {
                throw new ArgumentNullException("Não existe nenhum formulário!");
            }

            return list;
        }

        public async Task<Form> GetFormsByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!", nameof(id));
            }

            Form? forms = await _formsRepository.GetFormsByIdAsync(id);

            if (forms == null)
            {
                throw new KeyNotFoundException("Id não encontrado!");
            }

            forms.Questions = await _questionRepository.GetQuestionByFormsId(id);

            return forms;
        }

        public async Task<Form> CreateFormsAsync(Form forms)
        {
            if (string.IsNullOrWhiteSpace(forms.Name))
            {
                throw new ArgumentException("O nome do formulário é inválido!", forms.Name);
            }

            FormGroup? FkFormGroup = await _formGroupRepository.GetFormsGroupByIdAsync(forms.Id);

            if(FkFormGroup == null)
            {
                throw new KeyNotFoundException("FK de grupo de formulário inválida!");
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
                throw new ArgumentException("ID inválido!", nameof(id));
            }

            Form? updatedForm = await _formsRepository.GetFormsByIdAsync(id);

            if (updatedForm == null)
            {
                throw new KeyNotFoundException("Id não encontrado!");
            }

            FormGroup? FkFormGroup = await _formGroupRepository.GetFormsGroupByIdAsync(forms.Id);

            if (FkFormGroup == null)
            {
                throw new KeyNotFoundException("FK de grupo de formulário inválida!");
            }

            if (string.IsNullOrWhiteSpace(forms.Name))
            {
                throw new ArgumentException("O nome do formulário é inválido!", forms.Name);
            }

            return await _formsRepository.UpdateFormsAsync(id, forms);
        }

        public async Task<bool> DeleteFormsAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!", nameof(id));
            }

            Form? deletedForm = await _formsRepository.GetFormsByIdAsync(id);

            if (deletedForm == null)
            {
                throw new KeyNotFoundException("Id não encontrado!");
            }

            List<Question> questions = await _questionRepository.GetQuestionByFormsId(id);

            foreach (Question question in questions)
            {
                await _answerRepository.DeleteAnswerByQuestionId(question.Id);
                await _questionRepository.DeleteQuestionsAsync(question.Id);
            }

            return await _formsRepository.DeleteFormsAsync(id);
        }
    }
}
