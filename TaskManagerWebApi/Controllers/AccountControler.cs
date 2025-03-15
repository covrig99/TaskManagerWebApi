using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TaskManagerWebApi.AuthorizationPolicy;
using TaskManagerWebApi.Controllers.ControllerHelper;
using TaskManagerWebApi.DTO_s.AccountDTO_s;
using TaskManagerWebApi.JWT_Helper;
using TaskManagerWebApi.Models;
using TaskManagerWebApi.Service.Interfaces;

namespace TaskManagerWebApi.Controllers
{

    [Route("[controller]/[Action]")]
    public class AccountControler : ControllerResponseHelper
    {
        private readonly IAccountService accountService;
        private readonly IMapper mapper;

        public AccountControler(IAccountService accountService, IMapper mapper)
        {
            this.accountService = accountService;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginInfoRequest userLoginInfoRequest)
        {
            try
            {
                var userLoginInfo = mapper.Map<UserDto>(userLoginInfoRequest);

                var jwtToken = await accountService.Login(userLoginInfo);
                return Ok(jwtToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }

        [HttpPost]
        [Authorize(AuthorizationPoilicyConstants.ADMIN_POLICY)]
        public async Task<IActionResult> CreateUser([FromBody] UserAddRequest userAddRequest)
        {
            var addedUser = mapper.Map<User>(userAddRequest);
            addedUser.PasswordHash = new PasswordHasher<User>().HashPassword(addedUser, userAddRequest.Password);

            var account = await accountService.CreateAccount(addedUser, userAddRequest.Password);
            if (account == null)
            {
                ModelState.AddModelError("", "Something went wrong when adding a user");
                return StatusCode(500, ModelState);
            }

            return Ok(account.Value);
        }

        [HttpPut("{userId}")]
        [Authorize(AuthorizationPoilicyConstants.ADMIN_POLICY)]

        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserUpdateRequest updatedUserRequest)
        {
            try
            {
                
                var userEntity = mapper.Map<User>(updatedUserRequest);

                var result = await accountService.UpdateUser(userId, userEntity, updatedUserRequest.NewPassword);

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Errors);
                }

                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch]
        [Authorize(AuthorizationPoilicyConstants.MANAGER_OR_USER_POLICY)]

        public async Task<IActionResult> UpdatePassword([FromBody][Required] PasswordUpdateRequest passwordUpdateRequest)
        {
            try
            {

                string newPassword = passwordUpdateRequest.NewPassword;
                var userLoginInfo = mapper.Map<UserDto>(passwordUpdateRequest);
                userLoginInfo.Email = User.GetUserEmail();
                var account = await accountService.UpdatePassword(userLoginInfo, newPassword);
                ChangeResponsePassword(account.Value);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[Authorize(AuthorizationPoilicyConstants.MANAGER_OR_ADMIN_POLICY)]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (users, totalUsers) = await accountService.GetAllUsers(request);

            if (!users.Any())
                return NotFound("No users found.");

            var usersGet = users.Select(user => mapper.Map<UserAuthenticated>(user)).ToList();

            var response = new
            {
                TotalCount = totalUsers,
                Offset = request.Offset,
                Limit = request.Limit,
                Users = usersGet
            };

            return Ok(response);
        }


    }


}
