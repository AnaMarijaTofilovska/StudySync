using System.ComponentModel.DataAnnotations;

namespace StudySync.DTOs
{
    public class CategoryUpdateDTO
    {
        [Required(ErrorMessage ="ID is required for updating")]
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
    }
}
