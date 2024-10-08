﻿using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;

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
                throw new ArgumentNullException("Não existe nenhuma pergunta!");
            }

            return list;
        }

        public async Task<Question> GetQuestionsByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!", nameof(id));
            }

            Question? questions = await _questionsRepository.GetQuestionsByIdAsync(id);

            if (questions == null)
            {
                throw new KeyNotFoundException($"Id {id} não encontrado!");
            }

            return questions;
        }

        public async Task<List<Question>> CreateQuestionsAsync(List<Question> questions)
        {
            foreach (Question question in questions)
            {
                if (string.IsNullOrWhiteSpace(question.Text))
                {
                    throw new ArgumentException("O texto da pergunta não pode ser vazio!", question.Text);
                }

                Form? FkForm = await _formRepository.GetFormsByIdAsync(question.IdForms);

                if (FkForm == null)
                {
                    throw new KeyNotFoundException($"FK do formulário {question.IdForms} inválida!");
                }

            }

            return await _questionsRepository.CreateQuestionsAsync(questions);
        }

        public async Task<bool> UpdateQuestionsAsync(int id, Question questions)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!", nameof(id));
            }

            Question? updateQuestion = await _questionsRepository.GetQuestionsByIdAsync(id);

            if (updateQuestion == null)
            {
                throw new KeyNotFoundException($"Id {id} não encontrado!");
            }

            Form? FkForm = await _formRepository.GetFormsByIdAsync(questions.IdForms);

            if (FkForm == null)
            {
                throw new KeyNotFoundException($"FK do formulário {questions.IdForms} inválida!");
            }

            if (string.IsNullOrWhiteSpace(questions.Text))
            {
                throw new ArgumentException("O texto da pergunta não pode ser vazio!", questions.Text);
            }

            return await _questionsRepository.UpdateQuestionsAsync(id, questions);
        }

        public async Task<bool> DeleteQuestionsAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!", nameof(id));
            }

            Question? deleteQuestion = await _questionsRepository.GetQuestionsByIdAsync(id);

            if (deleteQuestion == null)
            {
                throw new KeyNotFoundException($"Id {id} não encontrado!");
            }

            await _answerRepository.DeleteAnswerByQuestionId(id);

            return await _questionsRepository.DeleteQuestionsAsync(id);
        }

        public async Task<List<Question>> GetQuestionByFormsId(int idForms)
        {
            return await _questionsRepository.GetQuestionByFormsId(idForms);
        }

    }
}
