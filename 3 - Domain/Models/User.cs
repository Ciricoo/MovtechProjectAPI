using MovtechProject.Domain.Models.Enums;
using System.Text.Json.Serialization;

namespace MovtechProject.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserEnumType Type { get; set; }
    }
}
