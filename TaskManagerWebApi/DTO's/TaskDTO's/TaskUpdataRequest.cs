namespace TaskManagerWebApi.DTO_s.TaskDTO_s
{
    public class TaskUpdataRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }

        public int UserId { get; set; }
    }
}
