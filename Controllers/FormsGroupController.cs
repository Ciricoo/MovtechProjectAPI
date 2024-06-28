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
            _formsGroupService = formsGroupService ?? throw new ArgumentNullException(nameof(formsGroupService));
        }

        [HttpGet]
        public async Task<ActionResult<List<FormsGroup>>> GetFormsGroup()
        {
            List<FormsGroup> formsGroups = await _formsGroupService.GetFormsGroupsAsync();
            return Ok(formsGroups);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FormsGroup>> GetFormsGroupById(int id)
        {
            FormsGroup formsGroup = await _formsGroupService.GetFormsGroupByIdAsync(id);
            if (formsGroup == null)
            {
                return NotFound();
            }
            return Ok(formsGroup);
        }

        [HttpPost]
        public async Task<ActionResult<FormsGroup>> CreateFormsGroup(FormsGroup formsGroup) 
        {
            int insertedId = await _formsGroupService.CreateFormsGroupAsync(formsGroup);
            formsGroup.Id = insertedId;

            return Ok(formsGroup);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateFormsGroup(int id, FormsGroup formsGroup)
        {
            bool updated = await _formsGroupService.UpdateFormsGroupAsync(id, formsGroup);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteFormsGroup(int id)
        {
            bool deleted = await _formsGroupService.DeleteFormsGroupAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok(deleted);
        }
    }
}
