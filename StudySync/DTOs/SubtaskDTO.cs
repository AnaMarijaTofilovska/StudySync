using StudySync.Models;
using System.ComponentModel.DataAnnotations;

namespace StudySync.DTOs
{
    public class SubtaskDTO
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public bool isCompleted { get; set; }
        public string TaskTitle { get; set; }
        
    }
}
