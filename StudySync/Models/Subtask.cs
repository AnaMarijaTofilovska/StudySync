using System.ComponentModel.DataAnnotations;

namespace StudySync.Models
{
    public class Subtask
    {
        [Key] 
        public int Id { get; set; }
        public string Title { get; set; }
        public bool isCompleted { get; set; }   

        
        //Foregin key to link to a parent taks  : One task can belong to one subtask
        public int TaskId {  get; set; }    
        public Taskitem Tasks { get; set; }
    }
}
