using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovtechProject.Models;
using MovtechProject.Services;

namespace MovtechProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsGroupController : ControllerBase
    {
        private readonly FormsGroupService _formsGroupService;

        public FormsGroupController(FormsGroupService formsGroupService)
        {
            _formsGroupService = formsGroupService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<FormsGroup>>> GetFormsGroup()
        {
            List<FormsGroup> get = await _formsGroupService.GetFormsGroupsAsync();

            return Ok(get);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<FormsGroup>> GetFormsGroupById(int id)
        {
            FormsGroup getId = await _formsGroupService.GetFormsGroupByIdAsync(id);
           
            return Ok(getId);
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<FormsGroup>> CreateFormsGroup(FormsGroup formsGroup) 
        {
            FormsGroup created = await _formsGroupService.CreateFormsGroupAsync(formsGroup);

            return Ok(created);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Administrador")]
        public async Task<ActionResult<bool>> UpdateFormsGroup(int id, FormsGroup formsGroup)
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
