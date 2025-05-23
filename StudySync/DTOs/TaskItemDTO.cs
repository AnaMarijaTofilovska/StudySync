namespace StudySync.DTOs
{
    //DTO For reading detailed task information
    public class TaskItemDTO
    {
        //This is data container which i want to push to user and controller 
        //Which columns do i want to show over the network
        public int Id { get; set; } 
        public string Title { get; set; }   
        public string Description { get; set; } 
        public DateTime DueDate { get; set; }
        public string Priority { get; set; }
        public bool IsCompleted { get; set; }



        // Include names from related entities
        public string UserName { get; set; }
        public string CategoryName { get; set; }

    }

}
