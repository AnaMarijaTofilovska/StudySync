using StudySync.Models;
using System.ComponentModel.DataAnnotations;

namespace StudySync.DTOs
{
    public class SubtaskUpdateDTO
    {
        [Required(ErrorMessage ="Id is required for updateing")]
        public int Id { get; set; }
        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }

        public bool isCompleted { get; set; }

        [Required(ErrorMessage ="Task Id is required")]
        public int TaskId { get; set; }
        
    }
}
