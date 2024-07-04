using MovtechProject.Models;
using MovtechProject.Repositories;

namespace MovtechProject.Services
{
    public class QuestionsService
    {
        private readonly QuestionsRepository _questionsRepository;
        public QuestionsService(QuestionsRepository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        public async Task<List<Questions>> GetQuestionsAsync()
        {
            var list = await _questionsRepository.GetQuestionsAsync();

            if (list == null)
            {
                throw new ArgumentException("Não existe nenhuma pergunta!");
            }

            return list;
        }

        public async Task<Questions> GetQuestionsByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            Questions? questions = await _questionsRepository.GetQuestionsByIdAsync(id);

            if (questions == null)
            {
                throw new ArgumentException("Id não encontrado!");
            }

            return questions;
        }

        //public async Task<Questions> CreateQuestionsAsync(Questions questions)
        //{
        //    if (string.IsNullOrWhiteSpace(questions.Text))
        //    {
        //        throw new ArgumentException("O texto da pergunta é inválido!");
        //    }

        //    return await _questionsRepository.CreateQuestionsAsync(questions,);
        //}

        public async Task<bool> UpdateQuestionsAsync(int id, Questions questions)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            if (string.IsNullOrWhiteSpace(questions.Text) || questions.Text.Length > 100)
            {
                throw new ArgumentException("O texto da pergunta é inválido!");
            }

            return await _questionsRepository.UpdateQuestionsAsync(id, questions);
        }

        public async Task<bool> DeleteQuestionsAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            return await _questionsRepository.DeleteQuestionsAsync(id);
        }
    }
}
