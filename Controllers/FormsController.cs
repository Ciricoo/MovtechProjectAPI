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
            _formsService = formsService ?? throw new ArgumentNullException(nameof(formsService));
        }

        [HttpGet]
        public async Task<ActionResult<List<Forms>>> GetForms()
        {
            List<Forms> forms = await _formsService.GetFormsAsync();
            return Ok(forms);    
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Forms>> GetFormsById(int id)
        {
            Forms forms = await _formsService.GetFormularioByIdAsync(id);
            if(forms == null)
            {
                return NotFound();
            }
            return Ok(forms);
        }

        [HttpPost]
        public async Task<ActionResult<Forms>> CreateForms(Forms forms)
        {
           int insertedId = await _formsService.CreateFormsAsync(forms);
           forms.Id = insertedId;

            return Ok(forms);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateForms(int id, Forms form)
        {
            bool updated = await _formsService.UpdateFormsAsync(id, form);
            if (!updated)
            {
                return NotFound();
            }
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteForms(int id)
        {
            bool deleted = await _formsService.DeleteFormsAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok(deleted);
        }
    }
}
