using System.ComponentModel.DataAnnotations;

namespace TaskManagerWebApi.DTO_s.AccountDTO_s
{
    public class UserUpdateRequest
    {
        [Required]
        public string Email { get; set; } 
        [Required]
        public string Role { get; set; } 

        public string? NewPassword { get; set; }
    }
}
