using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DTO_s.TaskDTO_s
{
    public class UpdateTaskStatusRequest
    {
        public TaskStatuses Status { get; set; }
        public string? RejectionReason { get; set; }
       
    }
}
