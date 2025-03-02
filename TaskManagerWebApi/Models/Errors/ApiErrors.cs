using FluentResults;
using Microsoft.AspNetCore.Http;

namespace TaskManagerWebApi.Models.Errors
{
    public class ApiErrors : Error
    {
        public int StatusCode { get; private set; }
        private ApiErrors(string ErrorMessage, int StatusCode) : base(ErrorMessage)
        {
            this.StatusCode = StatusCode;
        }
        public static ApiErrors ValueIsNull { get { return new ApiErrors("ValueIsNull", StatusCodes.Status400BadRequest); } }
        public static ApiErrors TaskNotAvailable { get { return new ApiErrors("TaskNotAvailable", StatusCodes.Status404NotFound); } }
        public static ApiErrors ParametersNotValidWithID { get { return new ApiErrors("ParametersNotValidWithID", StatusCodes.Status400BadRequest); } }
        public static ApiErrors ParametersNotValidWithoutID { get { return new ApiErrors("ParametersNotValidWithoutID", StatusCodes.Status400BadRequest); } }
        public static ApiErrors TimeSpanNotValid { get { return new ApiErrors("TimeSpanNotValid", StatusCodes.Status400BadRequest); } }
        public static ApiErrors NotEnoughRights { get { return new ApiErrors("NotEnoughRights", StatusCodes.Status403Forbidden); } }
        public static ApiErrors WrongPassword { get { return new ApiErrors("WrongPassword", StatusCodes.Status401Unauthorized); } }
        public static ApiErrors InvalidEmail { get { return new ApiErrors("InvalidEmail", StatusCodes.Status404NotFound); } }
    }
}
