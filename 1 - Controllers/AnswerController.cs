using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovtechProject.Domain.Models;
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
        public async Task<ActionResult<List<Answer>>> GetAnswers()
        {
            List<Answer> get = await _answerService.GetAnswersAsync();
            return Ok(get);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<Answer>> GetAnswersById(int id)
        {
            Answer getId = await _answerService.GetAnswersByIdAsync(id);
            return Ok(getId);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Answer>> CreateAnswers(Answer answers)
        {
            Answer created = await _answerService.CreateAnswersAsync(answers);
            return Ok(created);
        }

    }
}
