using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DTO_s.TaskDTO_s
{
    public class UpdateTaskStatusRequest
    {
        public string Status { get; set; }
        public string RejectionReason { get; set; }
       
    }
}
