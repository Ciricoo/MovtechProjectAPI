namespace MovtechProject.Domain.Models
{
    public class Form
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int IdFormsGroup { get; set; }

        //Usado para indicar que Formulario possui uma coleção de Perguntas 
        public ICollection<Question> Questions { get; set; }

        public Form()
        {
            Questions = new List<Question>();
        }
    }
}
