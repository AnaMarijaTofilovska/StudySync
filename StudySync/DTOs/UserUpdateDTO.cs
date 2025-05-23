using System.ComponentModel.DataAnnotations;

namespace StudySync.DTOs
{
    public class UserUpdateDTO
    {
        [Required(ErrorMessage = "ID is required for updates")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
