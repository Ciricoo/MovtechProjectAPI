using MovtechProject.Models;
using MovtechProject.Repositories;

namespace MovtechProject.Services
{
    public class FormsGroupService
    {
        private readonly FormsGroupRepository _formsGroupRepository;

        public FormsGroupService(FormsGroupRepository formsGroupRepository)
        {
            _formsGroupRepository = formsGroupRepository;
        }

        public async Task<List<FormsGroup>> GetFormsGroupsAsync()
        {
            var list = await _formsGroupRepository.GetFormsGroupsAsync();

            if(list == null || list.Count == 0)
            {
                throw new ArgumentException("Não existe grupo de formulários!");
            }

            return list;
        }

        public async Task<FormsGroup> GetFormsGroupByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            FormsGroup? formsGroup = await _formsGroupRepository.GetFormsGroupByIdAsync(id);

            if (formsGroup == null)
            {
                throw new ArgumentException("Id não encontrado!");
            }

            return formsGroup;
        }

        public async Task<FormsGroup> CreateFormsGroupAsync(FormsGroup formsGroup)
        {
            if (string.IsNullOrWhiteSpace(formsGroup.Name) || formsGroup.Name.Length > 100)
            {
                throw new ArgumentException("O nome do grupo é inválido!");
            }

            return await _formsGroupRepository.CreateFormsGroupAsync(formsGroup);
        }

        public async Task<bool> UpdateFormsGroupAsync(int id, FormsGroup formsGroup)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            if (string.IsNullOrWhiteSpace(formsGroup.Name) || formsGroup.Name.Length > 100)
            {
                throw new ArgumentException("O nome do grupo de formulários é inválido!");
            }

            return await _formsGroupRepository.UpdateFormsGroupAsync(id, formsGroup);
        }

        public async Task<bool> DeleteFormsGroupAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID inválido!");
            }

            return await _formsGroupRepository.DeleteFormsGroupAsync(id);
        }
    }
}
