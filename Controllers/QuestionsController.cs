using Microsoft.AspNetCore.Http;
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
        public QuestionsController(QuestionsService questionsService) {
            _questionsService = questionsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Questions>>> GetQuestions()
        {
            List<Questions> questions = await _questionsService.GetQuestionsAsync();
            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Questions>> GetQuestionsById(int id)
        {
            Questions questions = await _questionsService.GetQuestionsByIdAsync(id);
            if(questions == null)
            {
                return NotFound();
            }
            return Ok(questions);
        }
    }
}
