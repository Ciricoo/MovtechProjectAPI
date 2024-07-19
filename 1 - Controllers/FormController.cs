using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovtechProject.Domain.Models;
using MovtechProject.Services;

namespace MovtechProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        private readonly FormService _formsService;

        public FormController(FormService formsService)
        {
            _formsService = formsService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Form>>> GetForms()
        {
            List<Form> get = await _formsService.GetFormsAsync();
            return Ok(get);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Form>> GetFormsById(int id)
        {
            Form getId = await _formsService.GetFormsByIdAsync(id);
            return Ok(getId);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<Form>> CreateForms(Form forms)
        {
            Form created = await _formsService.CreateFormsAsync(forms);
            return Ok(created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> UpdateForms(int id, Form form)
        {
            bool updated = await _formsService.UpdateFormsAsync(id, form);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteForms(int id)
        {
            bool deleted = await _formsService.DeleteFormsAsync(id);
            return Ok(deleted);
        }
    }
}