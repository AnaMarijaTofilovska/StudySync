using System.ComponentModel.DataAnnotations;

namespace StudySync.DTOs
{
    public class SubtaskCreateDTO
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public bool isCompleted { get; set; }

        [Required(ErrorMessage = "Task Id is required")]
        public int TaskId { get; set; }
    }
}
