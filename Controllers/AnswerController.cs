using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovtechProject.Models;
using MovtechProject.Services;

namespace MovtechProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly AnswerService _answerService;

        public AnswerController (AnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<List<Answers>>> GetAnswers()
        {
            List<Answers> get = await _answerService.GetAnswersAsync();
            return Ok(get);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<Answers>> GetAnswersById(int id)
        {
            Answers getId = await _answerService.GetAnswersByIdAsync(id);

            return Ok(getId);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Answers>> CreateAnswers(Answers answers)
        {
            Answers created = await _answerService.CreateAnswersAsync(answers);

            return Ok(created);
        }

    }
}
