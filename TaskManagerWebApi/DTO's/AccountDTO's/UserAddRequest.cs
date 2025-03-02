namespace TaskManagerWebApi.DTO_s.AccountDTO_s
{
    public class UserAddRequest
    {
        public UserLoginInfoRequest UserLoginInfo { get; set; }
        public int Type { get; set; }
    }
}
