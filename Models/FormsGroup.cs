using System.Collections.ObjectModel;

namespace MovtechProject.Models
{
    public class FormsGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        
        // Usado para indicar que Grupo de Formulário possui uma coleção de Formulários
        public ICollection <Forms> Forms { get; set; }

        public FormsGroup()
        {
            Forms = new List<Forms>();
        }
    }
}
