using System.ComponentModel.DataAnnotations;

namespace TaskManagerWebApi.DTO_s.AccountDTO_s
{
    public class UserLoginInfoRequest
    {
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
