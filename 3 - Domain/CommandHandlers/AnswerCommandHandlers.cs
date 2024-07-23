﻿using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;
using System.Collections.Generic;

namespace MovtechProject._3___Domain.CommandHandlers
{
    public class AnswerCommandHandlers
    {
        private readonly UserRepository _userRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly AnswerRepository _answerRepository;

        public AnswerCommandHandlers(UserRepository userRepository,QuestionRepository questionRepository, AnswerRepository answerRepository)
        {
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }

        public async Task<List<Answer>> GetAnswersAsync()
        {
            List<Answer> list = await _answerRepository.GetAnswersAsync();

            if (!list.Any())
            {
                throw new ArgumentNullException("Não existe nenhuma resposta!");
            }

            return list;
        }

        public async Task<Answer> GetAnswersByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!", nameof(id));
            }

            Answer? answers = await _answerRepository.GetAnswersByIdAsync(id);

            if (answers == null)
            {
                throw new KeyNotFoundException($"Id {id} não encontrado!");
            }

            return answers;
        }

        public async Task<Answer> CreateAnswersAsync(Answer answers)
        {
            if (answers.Grade < 0 || answers.Grade > 10)
            {
                throw new ArgumentException("Nota inválida!", nameof(answers.Grade));
            }

            Question? FkQuestion = await _questionRepository.GetQuestionsByIdAsync(answers.IdQuestion);

            if (FkQuestion == null)
            {
                throw new KeyNotFoundException($"FK de perguntas {answers.IdQuestion} inválida!");
            }

            User? FkUser = await _userRepository.GetUserByIdAsync(answers.IdUser);

            if (FkUser == null)
            {
                throw new KeyNotFoundException($"FK de usuário {answers.IdUser} inválida!");
            }

            return await _answerRepository.CreateAnswersAsync(answers);
        }

        public async Task<List<Answer>> GetAnswerByUserIdAsync(int idUser)
        {
            List<Answer> answers = await _answerRepository.GetAnswersAsync();

            if (!answers.Any())
            {
                throw new ArgumentNullException("Não existe nenhuma resposta!");
            }

            if (idUser <= 0)
            {
                throw new ArgumentException("ID inválido!", nameof(idUser));
            }

            User? user = await _userRepository.GetUserByIdAsync(idUser);

            if (user == null)
            {
                throw new KeyNotFoundException($"Id {idUser} do usuário não encontrado!");
            }

            List<Answer> listUserId = await _answerRepository.GetAnswerByUserIdAsync(idUser);

            if(!listUserId.Any())
            {
                throw new ArgumentNullException("Esse usuário não possui nenhuma resposta!");
            }

            return listUserId;
        }
    }
}
