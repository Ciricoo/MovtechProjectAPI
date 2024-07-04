namespace MovtechProject.Models
{
    public class Questions
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int IdForms { get; set; }

        // Usado para indicar que Perguntas tem uma coleção de respostas
        public ICollection<Answers> Answers { get; set; }

        public Questions()
        {
            Answers = new List<Answers>();
        }
    }
}
