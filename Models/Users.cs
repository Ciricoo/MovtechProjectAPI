using MovtechProject.Models.Enums;

namespace MovtechProject.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserEnumType Type { get; set; }
    }
}
