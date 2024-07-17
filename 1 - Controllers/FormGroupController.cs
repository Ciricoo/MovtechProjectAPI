using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovtechProject.Domain.Models;
using MovtechProject.Services;

namespace MovtechProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormGroupController : ControllerBase
    {
        private readonly FormGroupService _formsGroupService;

        public FormGroupController(FormGroupService formsGroupService)
        {
            _formsGroupService = formsGroupService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<FormGroup>>> GetFormsGroup()
        {
            List<FormGroup> get = await _formsGroupService.GetFormsGroupsAsync();
            return Ok(get);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<FormGroup>> GetFormsGroupById(int id)
        {
            FormGroup getId = await _formsGroupService.GetFormsGroupByIdAsync(id);
            return Ok(getId);
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<FormGroup>> CreateFormsGroup(FormGroup formsGroup) 
        {
            FormGroup created = await _formsGroupService.CreateFormsGroupAsync(formsGroup);
            return Ok(created);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<bool>> UpdateFormsGroup(int id, FormGroup formsGroup)
        {
            bool updated = await _formsGroupService.UpdateFormsGroupAsync(id, formsGroup);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<bool>> DeleteFormsGroup(int id)
        {
            bool deleted = await _formsGroupService.DeleteFormsGroupAsync(id);
            return Ok(deleted);
        }
    }
}
