using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovtechProject.Models;
using MovtechProject.Services;

namespace MovtechProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly QuestionsService _questionsService;
        public QuestionsController(QuestionsService questionsService)
        {
            _questionsService = questionsService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Questions>>> GetQuestions()
        {
            List<Questions> get = await _questionsService.GetQuestionsAsync();

            return Ok(get);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Questions>> GetQuestionsById(int id)
        {
            Questions getId = await _questionsService.GetQuestionsByIdAsync(id);

            return Ok(getId);
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<Questions>> CreateQuestions(Questions questions)
        {
            Questions created = await _questionsService.CreateQuestionsAsync(questions);

            return Ok(created);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<Questions>> UpdateQuestions(int id, Questions questions)
        {
            bool updated = await _questionsService.UpdateQuestionsAsync(id, questions);

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<Questions>> DeleteQuestions(int id)
        {
            bool deleted = await _questionsService.DeleteQuestionsAsync(id);

            return Ok(deleted);
        }
    }
}
