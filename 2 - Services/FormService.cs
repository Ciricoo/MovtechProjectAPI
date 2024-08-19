using MovtechProject._3___Domain.CommandHandlers;
using MovtechProject.DataAcess.Repositories;
using MovtechProject.Domain.Models;

namespace MovtechProject.Services
{
    public class FormService
    {
        private readonly FormCommandHandlers _formCommandHandlers;

        public FormService(FormCommandHandlers formCommandHandlers)
        {
            _formCommandHandlers = formCommandHandlers;
        }
        public async Task<List<Form>> GetFormsAsync()
        {
            return await _formCommandHandlers.GetFormsAsync();
        }

        public async Task<Form> GetFormsByIdAsync(int id)
        {
            return await _formCommandHandlers.GetFormsByIdAsync(id);
        }

        public async Task<Form> CreateFormsAsync(Form forms)
        {
            return await _formCommandHandlers.CreateFormsAsync(forms);
        }

        public async Task<bool> UpdateFormsAsync(int id, Form forms)
        {
            return await _formCommandHandlers.UpdateFormsAsync(id, forms);
        }

        public async Task<bool> DeleteFormsAsync(int id)
        {
            return await _formCommandHandlers.DeleteFormsAsync(id);
        }

        public async Task<List<Form>> GetFormsByGroupId(int idGroup)
        {
            return await _formCommandHandlers.GetFormsByGroupId(idGroup);
        }
    }
}

