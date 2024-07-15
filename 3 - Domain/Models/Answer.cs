namespace MovtechProject.Domain.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int Grade { get; set; }
        public string Description { get; set; } = string.Empty;
        public int IdQuestion { get; set; }
        public int IdUser { get; set; }

    }
}
