using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovtechProject.Models;
using MovtechProject.Services;


namespace MovtechProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly FormsService _formsService;

        public FormsController(FormsService formsService)
        {
            _formsService = formsService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Forms>>> GetForms()
        {
            List<Forms> get = await _formsService.GetFormsAsync();
            return Ok(get);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Forms>> GetFormsById(int id)
        {
            Forms getId = await _formsService.GetFormsByIdAsync(id);
            return Ok(getId);
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<Forms>> CreateForms(Forms forms)
        {
            Forms created = await _formsService.CreateFormsAsync(forms);
            return Ok(created);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult> UpdateForms(int id, Forms form)
        {
            bool updated = await _formsService.UpdateFormsAsync(id, form);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult> DeleteForms(int id)
        {
            bool deleted = await _formsService.DeleteFormsAsync(id);
            return Ok(deleted);
        }
    }
}