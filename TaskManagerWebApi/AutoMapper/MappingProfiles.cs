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
            CreateMap<UserTask, TaskGetRequest>();
            CreateMap<TaskAddRequest, UserTask>();
            CreateMap<TaskUpdateRequest, UserTask>();
            CreateMap<UserLoginInfoRequest, User>();
            CreateMap<UserAddRequest, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email)) // Ensure UserName is set
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<UserLoginInfoRequest, UserDto>();
            CreateMap<User, UserAddRequest>();
            CreateMap<UserLoginInfo, UserAuthenticated>();
            CreateMap<AccountDeleteRequest, UserLoginInfoRequest>();
            CreateMap<PasswordUpdateRequest, UserDto>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
            CreateMap<User, UserAuthenticated>();
            CreateMap<AssignTaskByManagerRequest,UserTask>();
        }

    }
}
