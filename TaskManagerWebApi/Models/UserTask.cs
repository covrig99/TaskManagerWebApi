using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerWebApi.Models
{
    
    public class UserTask
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public TaskStatuses Status { get; set; } = TaskStatuses.ToDo;
        public string? RejectionReason { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int IdUser { get; set; }
        public User User { get; set; }

    }
}
