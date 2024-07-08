using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovtechProject.Models;
using MovtechProject.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        private bool IsAuthenticated(out string userType)
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
            {
                return UsersController.TryGetUserType(token.ToString(), out userType);
            }

            userType = string.Empty;
            return false;
        }

        private bool IsAuthorized(string requiredRole)
        {
            if (IsAuthenticated(out string userType))
            {
                if (userType == requiredRole || userType == "Administrador")
                {
                    return true;
                }
            }
            return false;
        }
         
        [HttpGet]
        public async Task<ActionResult<List<Forms>>> GetForms()
        {
            if (!IsAuthenticated(out _))
            {
                return Unauthorized("Usuário não autenticado");
            }

            List<Forms> get = await _formsService.GetFormsAsync();
            return Ok(get);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Forms>> GetFormsById(int id)
        {
            if (!IsAuthenticated(out _))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            Forms getId = await _formsService.GetFormsByIdAsync(id);
            return Ok(getId);
        }

        [HttpPost]
        public async Task<ActionResult<Forms>> CreateForms(Forms forms)
        {
            if (!IsAuthorized("Administrador"))
            {
                return Unauthorized("Acesso negado. Apenas administradores podem criar formulários.");
            }

            Forms created = await _formsService.CreateFormsAsync(forms);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateForms(int id, Forms form)
        {
            if (!IsAuthorized("Administrador"))
            {
                return Unauthorized("Acesso negado. Apenas administradores podem atualizar formulários.");
            }

            bool updated = await _formsService.UpdateFormsAsync(id, form);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteForms(int id)
        {
            if (!IsAuthorized("Administrador"))
            {
                return Unauthorized("Acesso negado. Apenas administradores podem excluir formulários.");
            }

            bool deleted = await _formsService.DeleteFormsAsync(id);
            return Ok(deleted);
        }
    }
}
