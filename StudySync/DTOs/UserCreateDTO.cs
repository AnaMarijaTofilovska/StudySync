using System.ComponentModel.DataAnnotations;

namespace StudySync.DTOs
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }
    }
}
