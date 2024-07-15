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
            List<FormGroup> list = await _formGroupCommandHandlers.GetFormsGroupsAsync();
            return list;
        }

        public async Task<FormGroup> GetFormsGroupByIdAsync(int id)
        {
            FormGroup formGroupById = await _formGroupCommandHandlers.GetFormsGroupByIdAsync(id);
            return formGroupById;
        }

        public async Task<FormGroup> CreateFormsGroupAsync(FormGroup formsGroup)
        {
            
            FormGroup createdFormGroup = await _formGroupCommandHandlers.CreateFormsGroupAsync(formsGroup);
            return createdFormGroup;
        }

        public async Task<bool> UpdateFormsGroupAsync(int id, FormGroup formsGroup)
        {
            bool updatedFormGroup = await _formGroupCommandHandlers.UpdateFormsGroupAsync(id, formsGroup);
            return updatedFormGroup;
        }

        public async Task<bool> DeleteFormsGroupAsync(int id)
        {
            bool deleted = await _formGroupCommandHandlers.DeleteFormsGroupAsync(id);
            return deleted;
        }
    }
}
