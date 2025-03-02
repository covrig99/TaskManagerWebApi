using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerWebApi.Models
{
    
    public class User : IdentityUser<int>   
    {
        
        [Required]
        public string Role { get; set; }
        public ICollection<UserTask> AssignedTasks { get; set; }
    }
}
