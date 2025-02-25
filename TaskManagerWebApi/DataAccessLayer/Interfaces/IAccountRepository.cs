using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DataAccessLayer.Interfaces
{
    public interface IAccountRepository
    {
        Task<Users> CreateAccount(Users users);
        bool VerifyAccount(Users users);
        Task<Users> UpdateAccount(Users users);
        Task<Users> DeleteAccount(Users users);
        Task<List<Users>> GetAllUsers(Users users);
        Task<Users> GetUser(int userId);
        
        
    }
}
