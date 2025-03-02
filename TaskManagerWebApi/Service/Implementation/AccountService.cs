using Microsoft.AspNetCore.Identity;
using TaskManagerWebApi.DataAccessLayer.Interfaces;
using TaskManagerWebApi.Models;
using TaskManagerWebApi.Service.Interfaces;

namespace TaskManagerWebApi.Service.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITaskRepository _taskRepository;
        public AccountService(IAccountRepository accountRepository, ITaskRepository taskRepository)
        {
            _accountRepository = accountRepository;
            _taskRepository = taskRepository;
        }

        public async Task<User> CreateAccount(User user)
        {
            await _accountRepository.CreateAccount(user);
            return user;
        }

        public async Task<User> DeleteAccount(int users)
        {
            var accountFound = await _accountRepository.GetUser(users);
            await _accountRepository.DeleteAccount(accountFound);
            return accountFound;
        }

        public Task<List<User>> GetAllUsers(User users)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> Login(UserLoginInfo userLoginInfo)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateAccount(User users)
        {
            throw new NotImplementedException();
        }

        public Task<UserLoginInfo> UpdatePassword(UserLoginInfo userLoginInfo, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
