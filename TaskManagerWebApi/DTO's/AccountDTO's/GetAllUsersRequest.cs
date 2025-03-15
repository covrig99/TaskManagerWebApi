using System.ComponentModel.DataAnnotations;

namespace TaskManagerWebApi.DTO_s.AccountDTO_s
{
    public class GetAllUsersRequest
    {
        public string? Role { get; set; }
        public string? Search { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Offset must be 0 or greater.")]
        public int? Offset { get; set; } = 0;

        [Range(1, 100, ErrorMessage = "Limit must be between 1 and 100.")]
        public int? Limit { get; set; } = 10;
    }
}
