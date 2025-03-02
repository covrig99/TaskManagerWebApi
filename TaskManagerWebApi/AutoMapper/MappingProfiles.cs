using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TaskManagerWebApi.DTO_s.AccountDTO_s;
using TaskManagerWebApi.DTO_s.TaskDTO_s;
using TaskManagerWebApi.Models;

namespace TaskManagerWebApi.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<TaskGetRequest,UserTask >();
            CreateMap<TaskAddRequest, UserTask>();
            CreateMap<TaskUpdateRequest, UserTask>();
            CreateMap<UserLoginInfoRequest, UserLoginInfo>();
            CreateMap<UserLoginInfo, UserAuthenticated>();
            CreateMap<AccountDeleteRequest, UserLoginInfoRequest>();
            CreateMap<PasswordUpdateRequest, UserLoginInfoRequest>();

        }

    }
}
