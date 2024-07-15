namespace MovtechProject.Domain.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int IdForms { get; set; }

        // Usado para indicar que Perguntas tem uma coleção de respostas
        public ICollection<Answer> Answers { get; set; }

        public Question()
        {
            Answers = new List<Answer>();
        }
    }
}
