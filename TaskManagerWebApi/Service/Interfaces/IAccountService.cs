using FluentResults;
using Microsoft.AspNetCore.Identity;
using TaskManagerWebApi.DataAccessLayer.Interfaces;
using TaskManagerWebApi.DTO_s.AccountDTO_s;
using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.Service.Interfaces
{
    public interface IAccountService
    {
        Task<Result<string>> Login(UserDto userLoginInfo);
        Task<Result<User>> CreateAccount(User users, string password);
        
        
        Task<User> DeleteAccount(int user);
        Task<List<User>> GetAllUsers();
        Task<User> GetUser(int userId);
        Task<Result<User>> UpdateUser(int userId, User updatedUser, string? newPassword);
        Task<Result<User>> UpdatePassword(UserDto userLoginInfo, string newPassword);
        IQueryable<User> GetAllUsersQueryable();
    }
}
