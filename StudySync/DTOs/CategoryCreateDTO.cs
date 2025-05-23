using System.ComponentModel.DataAnnotations;

namespace StudySync.DTOs
{
    public class CategoryCreateDTO
    {
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
    }
}
