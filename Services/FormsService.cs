using MovtechProject.Models;
using MovtechProject.Repositories;

namespace MovtechProject.Services
{
    public class FormsService
    {
        private readonly FormsRepository _formsRepository;

        public FormsService(FormsRepository formsRepository)
        {
            _formsRepository = formsRepository;
        }


        public async Task<List<Forms>> GetFormsAsync()
        {
            var list = await _formsRepository.GetFormsAsync();

            if (list == null)
            {
                throw new ArgumentException("Não existe nenhum formulário!");
            }

            return list;
        }

        public async Task<Forms> GetFormsByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            Forms? forms = await _formsRepository.GetFormsByIdAsync(id);

            if (forms == null)
            {
                throw new ArgumentException("Id não encontrado!");
            }

            return forms;
        }

        public async Task<Forms> CreateFormsAsync(Forms forms)
        {
            if (string.IsNullOrWhiteSpace(forms.Name) || forms.Name.Length > 100)
            {
                throw new ArgumentException("O nome do formulário é inválido!");
            }

            return await _formsRepository.CreateFormsAsync(forms);
        }

        public async Task<bool> UpdateFormsAsync(int id, Forms forms)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            if (string.IsNullOrWhiteSpace(forms.Name) || forms.Name.Length > 100)
            {
                throw new ArgumentException("O nome do formulário é inválido!");
            }

            return await _formsRepository.UpdateFormsAsync(id, forms);
        }

        public async Task<bool> DeleteFormsAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            return await _formsRepository.DeleteFormsAsync(id);
        }
    }
}

