namespace TaskManagerWebApi.DTO_s.TaskDTO_s
{
    public class TaskGetRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public string AssignedUserId { get; set; }
        public int UserId { get; set; }

    }
}
