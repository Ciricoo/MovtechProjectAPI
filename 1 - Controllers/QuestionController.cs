using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovtechProject.Domain.Models;
using MovtechProject.Services;

namespace MovtechProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionService _questionsService;
        public QuestionController(QuestionService questionsService)
        {
            _questionsService = questionsService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Question>>> GetQuestions()
        {
            List<Question> get = await _questionsService.GetQuestionsAsync();
            return Ok(get);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Question>> GetQuestionsById(int id)
        {
            Question getId = await _questionsService.GetQuestionsByIdAsync(id);
            return Ok(getId);
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<Question>> CreateQuestions(Question questions)
        {
            Question created = await _questionsService.CreateQuestionsAsync(questions);
            return Ok(created);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<Question>> UpdateQuestions(int id, Question questions)
        {
            bool updated = await _questionsService.UpdateQuestionsAsync(id, questions);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<Question>> DeleteQuestions(int id)
        {
            bool deleted = await _questionsService.DeleteQuestionsAsync(id);
            return Ok(deleted);
        }
    }
}
