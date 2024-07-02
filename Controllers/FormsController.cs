using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<List<Forms>>> GetForms()
        {
            List<Forms> get = await _formsService.GetFormsAsync();

            return Ok(get);    
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Forms>> GetFormsById(int id)
        {
            Forms getId = await _formsService.GetFormsByIdAsync(id);

            return Ok(getId);
        }

        [HttpPost]
        public async Task<ActionResult<Forms>> CreateForms(Forms forms)
        {
           Forms created = await _formsService.CreateFormsAsync(forms);

            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateForms(int id, Forms form)
        {
            bool updated = await _formsService.UpdateFormsAsync(id, form);

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteForms(int id)
        {
            bool deleted = await _formsService.DeleteFormsAsync(id);

            return Ok(deleted);
        }
    }
}
