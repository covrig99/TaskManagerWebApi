using System.ComponentModel.DataAnnotations;

namespace TaskManagerWebApi.DTO_s.AccountDTO_s
{
    public class UserUpdateRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;

        public string? NewPassword { get; set; }
    }
}
