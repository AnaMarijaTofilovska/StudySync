using AutoMapper;
using StudySync.DTOs;
using StudySync.Models;

namespace StudySync.Profiles
{
    public class SubtaskProfiles : Profile
    {
        public SubtaskProfiles() 
        {
            CreateMap<Subtask, SubtaskDTO>();
            CreateMap<SubtaskCreateDTO,Subtask>();
            CreateMap<SubtaskUpdateDTO, Subtask>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }
}
