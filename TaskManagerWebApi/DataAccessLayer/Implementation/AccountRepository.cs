using TaskManagerWebApi.DataAccessLayer.Interfaces;
using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.DataAccessLayer.Implementation
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext context;
        public AccountRepository(DataContext context)
        {
            this.context = context;
        }

        public Task<Users> CreateAccount(Users users)
        {
            throw new NotImplementedException();
        }

        public Task<Users> DeleteAccount(Users users)
        {
            throw new NotImplementedException();
        }

        public Task<List<Users>> GetAllUsers(Users users)
        {
            throw new NotImplementedException();
        }

        public Task<Users> UpdateAccount(Users users)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyAccount(Users users)
        {
            throw new NotImplementedException();
        }
    }
}
