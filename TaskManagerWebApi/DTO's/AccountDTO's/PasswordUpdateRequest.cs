using System.ComponentModel.DataAnnotations;

namespace TaskManagerWebApi.DTO_s.AccountDTO_s
{
    public class PasswordUpdateRequest
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
