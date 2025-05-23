using System.ComponentModel.DataAnnotations;

namespace StudySync.Models
{
    public class Taskitem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public string Priority { get; set; } //Low,Medium,High
        public DateTime DueDate { get; set; }
        public bool isCompleted {  get; set; } = false;

        //Foregin Keys:

        //TO ONE!
        //Link Tasks to user,foregin key to 
        public int UserId { get; set; }
        public User User { get; set; }


        //Foregin key for categories, Link task to Category 
        public int CategoryId { get; set; } //links to navigated category table
        public Category Category { get; set; }  //

        
        //TO MULTIPLE!
        //One task can have multiple subtasks
        public List<Subtask> Subtasks { get; set; } = new();



    }
}
