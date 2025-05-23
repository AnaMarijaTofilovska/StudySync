
using AutoMapper;
using StudySync.DTOs;
using StudySync.Models;

namespace StudySync.Profiles
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<UserDTO, User>();
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserUpdateDTO, User>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
