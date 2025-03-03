using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DTO_s.TaskDTO_s
{
    public class TaskGetRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatuses Status { get; set; }
        public string AssignedUserId { get; set; }
        public int UserId { get; set; }

    }
}
