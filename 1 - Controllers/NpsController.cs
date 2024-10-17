using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovtechProject._2___Services;

namespace MovtechProject._1___Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NpsController : ControllerBase
    {

        private readonly NpsService _npsService;

        public NpsController(NpsService npsService)
        {
            _npsService = npsService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<int>> NpsScore()
        {
            int nps = await _npsService.NpsScore();
            return Ok(nps);
        }

        [HttpGet("Npslist")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<List<int>>> GetAnswersAccordingNpsGrade()
        {
            List<int> npsList = await _npsService.GetAnswersAccordingNpsGrade();
            return Ok(npsList);
        }

        [HttpGet("Group/{idGroup}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<int>> GetNpsByGroupId(int idGroup)
        {
            int nps = await _npsService.GetNpsByGroupId(idGroup);
            return Ok(nps);
        }
    }
}
