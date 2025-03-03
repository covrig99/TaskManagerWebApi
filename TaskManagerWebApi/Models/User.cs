using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskManagerWebApi.Models
{
    
    public class User : IdentityUser<int>   
    {
        
        [Required]
        public string? Role { get; set; }
        [JsonIgnore]
        public ICollection<UserTask> AssignedTasks { get; set; }
    }
}
