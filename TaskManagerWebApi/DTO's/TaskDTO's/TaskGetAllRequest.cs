using System.ComponentModel.DataAnnotations;
using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DTO_s.TaskDTO_s
{
    public class TaskGetAllRequest
    {
        public int? ManagerId { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public TaskStatuses? Status { get; set; }

        [RegularExpression("ASC|DESC", ErrorMessage = "SortDirection must be 'ASC' or 'DESC'.")]
        public string? SortDirection { get; set; }

        public string SortBy { get; set; } = "CreatedDate";

        [Range(0, int.MaxValue, ErrorMessage = "Offset must be at least 0.")]
        public int? Offset { get; set; }

        [Range(1, 100, ErrorMessage = "Limit must be between 1 and 100.")]
        public int? Limit { get; set; }
    }
}
