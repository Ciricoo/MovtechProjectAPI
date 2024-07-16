using MovtechProject._3___Domain.CommandHandlers;
using MovtechProject.Domain.Models;

namespace MovtechProject.Services
{
    public class FormGroupService
    {
        private readonly FormGroupCommandHandlers _formGroupCommandHandlers;

        public FormGroupService(FormGroupCommandHandlers formGroupCommandHandlers)
        {
            _formGroupCommandHandlers = formGroupCommandHandlers;
        }

        public async Task<List<FormGroup>> GetFormsGroupsAsync()
        {
            return await _formGroupCommandHandlers.GetFormsGroupsAsync();
        }

        public async Task<FormGroup> GetFormsGroupByIdAsync(int id)
        {
            return await _formGroupCommandHandlers.GetFormsGroupByIdAsync(id);
        }

        public async Task<FormGroup> CreateFormsGroupAsync(FormGroup formsGroup)
        {
            return await _formGroupCommandHandlers.CreateFormsGroupAsync(formsGroup);
        }

        public async Task<bool> UpdateFormsGroupAsync(int id, FormGroup formsGroup)
        {
            return await _formGroupCommandHandlers.UpdateFormsGroupAsync(id, formsGroup);
        }

        public async Task<bool> DeleteFormsGroupAsync(int id)
        {
            return await _formGroupCommandHandlers.DeleteFormsGroupAsync(id);
        }
    }
}
