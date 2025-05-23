using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudySync.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        //One user can have multiply tasks
        [JsonIgnore]
        public List<Taskitem> Tasks { get; set; } = new();
    }
}
