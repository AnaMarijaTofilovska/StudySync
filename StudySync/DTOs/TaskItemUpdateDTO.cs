using System.ComponentModel.DataAnnotations;

namespace StudySync.DTOs
{
    // DTO for updating existing tasks
    public class TaskItemUpdateDTO
    {
        [Required(ErrorMessage = "ID is required for updates")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Priority is required")]
        public string Priority { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        public int CategoryId { get; set; }

        // Allow updating the assigned user
        public int UserId { get; set; }

        // Include IsCompleted for task status updates
        public bool IsCompleted { get; set; }
    }
}
