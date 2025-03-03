using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagerWebApi.Models;
using TaskManagerWebApi.Models.Errors;
using TaskManagerWebApi.Models.UserRoles;

namespace TaskManagerWebApi.Controllers.ControllerHelper
{
    public class ControllerResponseHelper : ControllerBase
    {
        protected IActionResult ProcessError(List<IError> errors)
        {
            var problemDetails = new ValidationProblemDetails();
            foreach (var error in errors)
            {
                if (error is ApiErrors)
                {
                    var pd = (ApiErrors)error;
                    problemDetails.Status = pd.StatusCode;
                    problemDetails.Title = pd.Message;
                }
                else
                {
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "InternalServerError";
                }
            }
            return StatusCode(problemDetails.Status.Value, problemDetails);
        }
        protected static void ChangeResponsePassword(User userLoginInfo)
        {
            if(userLoginInfo == null || userLoginInfo.PasswordHash == null) return;
            userLoginInfo.PasswordHash = UserContatnts.DEFAULT_STRING;
        }
    }
}
