using System.ComponentModel.DataAnnotations;

namespace StudySync.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        //Foregin key to multiple Tasks : One category can have multiple tasks
        public List<Taskitem> Tasks { get; set; } = new();
    }
}
