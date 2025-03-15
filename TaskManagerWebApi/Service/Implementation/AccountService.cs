using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagerWebApi.DataAccessLayer.Interfaces;
using TaskManagerWebApi.DTO_s.AccountDTO_s;
using TaskManagerWebApi.Models;
using TaskManagerWebApi.Models.Errors;
using TaskManagerWebApi.Models.UserRoles;
using TaskManagerWebApi.Service.Interfaces;

namespace TaskManagerWebApi.Service.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IConfiguration config;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;


        public AccountService(IAccountRepository accountRepository, ITaskRepository taskRepository, IConfiguration config, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _accountRepository = accountRepository;
            _taskRepository = taskRepository;
            this.config = config;
        }
        //public IQueryable<User> GetAllUsersQueryable()
        //{
        //    return _accountRepository.GetAllUsersQueryable();
        //}
        private async Task EnsureRolesExist()
        {
            string[] roles = { "Admin", "Manager", "User" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int> { Name = role });
                }
            }
        }
        public async Task<Result<User>> CreateAccount(User user, string password)
        {
            if (user == null || string.IsNullOrWhiteSpace(password))
            {
                return Result.Fail(ApiErrors.ValueIsNull);
            }
            if (user.Role != UserContatnts.MANAGER_ROLE && user.Role != UserContatnts.USER_ROLE)
            {
                return Result.Fail(ApiErrors.InvalidRole);
            }
            await EnsureRolesExist();
            string roleName = user.Role ?? "User";
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return Result.Fail($"Invalid role: {roleName}. Choose from Admin, Manager, or User.");
            }
            user.UserName = user.Email;
            var createResult = await _userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                return Result.Fail($"User creation failed: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
            }


            await _userManager.AddToRoleAsync(user, roleName);

            return Result.Ok(user);
            //var passwordHasher = new PasswordHasher<User>();
            //user.PasswordHash = passwordHasher.HashPassword(user, password);
            //var createdUser = await _accountRepository.CreateAccount(user);

            //return Result.Ok(createdUser);
        }

        public async Task<User> DeleteAccount(int users)
        {
            var accountFound = await _accountRepository.GetUser(users);
            await _accountRepository.DeleteAccount(accountFound);
            return accountFound;
        }

        public async Task<(List<User> Users, int TotalCount)> GetAllUsers(GetAllUsersRequest request)
        {
            var usersQuery = _accountRepository.GetAllUsersQueryable();

            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                usersQuery = usersQuery.Where(u => u.Role == request.Role);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                usersQuery = usersQuery.Where(u => u.Email.Contains(request.Search) || u.UserName.Contains(request.Search));
            }

            int totalUsers = await usersQuery.CountAsync();

            var users = await usersQuery
                .Skip(request.Offset ?? 0)
                .Take(request.Limit ?? 10)
                .ToListAsync();

            return (users, totalUsers);
        }

        public Task<User> GetUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string>> Login(UserDto userLoginInfoRequest)
        {
            var userLoginInfoFind = await _accountRepository.FindByEmail(userLoginInfoRequest.Email);
            if (userLoginInfoFind == null)
            {
                return Result.Fail(ApiErrors.InvalidEmail);
            }

            var succes = await _userManager.CheckPasswordAsync(userLoginInfoFind, userLoginInfoRequest.Password);


            if (!succes)
            {
                return Result.Fail(ApiErrors.WrongPassword);
            }
            string role = userLoginInfoFind.Role;
            if (role == null)
            {
                return Result.Fail("Role not found");
            }

            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, userLoginInfoFind.UserName),
            new Claim(ClaimTypes.Email, userLoginInfoFind.Email),
            new Claim(ClaimTypes.Role, role)
            };




            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(3),
                signingCredentials: credentials);
            return Result.Ok(new JwtSecurityTokenHandler().WriteToken(token));

        }

        public async Task<Result<User>> UpdateUser(int userId, User updatedUser, string? newPassword)
        {
            var user = await _accountRepository.GetUser(userId);
            if (user == null)
                return Result.Fail("User not found");
            

            if (string.IsNullOrWhiteSpace(updatedUser.Role))
                return Result.Fail("Role cannot be empty");
            user.Email = updatedUser.Email;
            user.Role = updatedUser.Role;
            user.UserName = updatedUser.UserName;
            user.UserName = user.Email;

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                var passwordHasher = new PasswordHasher<User>();
                user.PasswordHash = passwordHasher.HashPassword(user, newPassword);
            }

            await _accountRepository.UpdateAccount(user);

            return Result.Ok(user);
        }

        public async Task<Result<User>> UpdatePassword(UserDto userLoginInfoRequest, string newPassword)
        {
            var userLoginInfoFound = await _accountRepository.FindByEmail(userLoginInfoRequest.Email);
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(userLoginInfoFound, userLoginInfoFound.PasswordHash, userLoginInfoRequest.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Result.Fail(ApiErrors.WrongPassword);
            }
            switch (userLoginInfoFound.Role)
            {
                case UserContatnts.ADMIN_ROLE:
                    {
                        var managerFound = await _accountRepository.FindByEmail(userLoginInfoFound.Email);
                        managerFound.PasswordHash = newPassword;
                        await _accountRepository.UpdateAccount(managerFound);
                    }
                    break;
                case UserContatnts.MANAGER_ROLE:
                    {
                        var managerFound = await _accountRepository.FindByEmail(userLoginInfoFound.Email);
                        managerFound.PasswordHash = newPassword;
                        await _accountRepository.UpdateAccount(managerFound);
                    }
                    break;
                case UserContatnts.USER_ROLE:
                    {
                        var managerFound = await _accountRepository.FindByEmail(userLoginInfoFound.Email);
                        managerFound.PasswordHash = newPassword;
                        await _accountRepository.UpdateAccount(managerFound);
                    }
                    break;
                default: return Result.Fail(ApiErrors.NotEnoughRights);
            }
            return Result.Ok(userLoginInfoFound);
        }
    }
}
