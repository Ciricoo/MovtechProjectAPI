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

        [HttpPost]
        public async Task<ActionResult<Questions>> CreateQuestions(Questions questions)
        {
            int insertedId = await _questionsService.CreateQuestionsAsync(questions);
            questions.Id = insertedId;

            return Ok(questions);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Questions>> UpdateQuestions(int id, Questions questions)
        {
            bool updated = await _questionsService.UpdateQuestionsAsync(id, questions);
            if(!updated)
            {
                return NotFound();
            }
            return Ok(questions);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Questions>> DeleteQuestions(int id)
        {
            bool deleted = await _questionsService.DeleteQuestionsAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok(deleted);
        }
    }
}
