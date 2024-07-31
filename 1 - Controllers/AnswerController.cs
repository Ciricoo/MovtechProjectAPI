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
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<List<Answer>>> GetAnswers()
        {
            List<Answer> get = await _answerService.GetAnswersAsync();
            return Ok(get);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<Answer>> GetAnswersById(int id)
        {
            Answer getId = await _answerService.GetAnswersByIdAsync(id);
            return Ok(getId);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<Answer>>> CreateAnswers(List<Answer> answers)
        {
            List<Answer> created = await _answerService.CreateAnswersAsync(answers, HttpContext);
            return Ok(created);
        }

        [HttpGet("UserId/{userId}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<List<Answer>>> GetAnswerByUserIdAsync(int userId)
        {
            List<Answer> get = await _answerService.GetAnswerByUserIdAsync(userId);
            return Ok(get);
        }
    }
}
