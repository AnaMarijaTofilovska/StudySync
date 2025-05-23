using System.ComponentModel.DataAnnotations;

namespace StudySync.Models
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }
        public DateTime ReminderDateTime {  get; set; }

        //[ModelName + Id]
        // Foregin Key to link to ONE Task : One Reminder can have one Task
        public int TaskId { get; set; }
        public Taskitem Tasks { get; set; }

    }
}


