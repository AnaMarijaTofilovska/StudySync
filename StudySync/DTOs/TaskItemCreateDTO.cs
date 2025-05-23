using System.ComponentModel.DataAnnotations;

namespace StudySync.DTOs
{
    public class TaskItemCreateDTO
    {
        //Columns to create , the controller sees this
        

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
       
        public string Description { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage ="Priority is required")]
        public string Priority {  get; set; }

        [Required(ErrorMessage ="Category ID is required")]
        public int CategoryId {  get; set; }

        [Required(ErrorMessage ="User ID is required")]
        public int UserId { get; set; }

    }
}
