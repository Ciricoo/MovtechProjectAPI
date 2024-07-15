using System.Collections.ObjectModel;

namespace MovtechProject.Domain.Models
{
    public class FormGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Usado para indicar que Grupo de Formulário possui uma coleção de Formulários
        public ICollection<Form> Forms { get; set; }

        public FormGroup()
        {
            Forms = new List<Form>();
        }
    }
}
