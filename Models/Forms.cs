namespace MovtechProject.Models
{
    public class Forms
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int IdFormsGroup { get; set; }

        //Usado para indicar que Formulario possui uma coleção de Perguntas 
        public ICollection<Questions>? Perguntas { get; set;}
    }
}
