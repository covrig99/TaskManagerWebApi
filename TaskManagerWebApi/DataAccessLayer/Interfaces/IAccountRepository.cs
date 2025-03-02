using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DataAccessLayer.Interfaces
{
    public interface IAccountRepository
    {
        Task<User> CreateAccount(User users);
        
        Task<User> UpdateAccount(User users);
        Task<User> DeleteAccount(User users);
        Task<List<User>> GetAllUsers(User users);
        Task<User> GetUser(int userId);
        
        
    }
}
