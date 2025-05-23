using AutoMapper;
using StudySync.DTOs;
using StudySync.Models;

namespace StudySync.Profiles
{
    public class TaskItemProfiles : Profile
    {
        //This is how it knows to which table to map
        public TaskItemProfiles()
        {
            // EXAMPLE 1: Domain to DTO (reading data)
            //Use here AutoMapper, to create mapping;my model shoukd be mapperd to my DTO 
            CreateMap<Taskitem, TaskItemDTO>();
            CreateMap<TaskItemCreateDTO, Taskitem>();

            // Add mapping for updates
            CreateMap<TaskItemUpdateDTO, Taskitem>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null)); // Only map non-null properties for partial updates
        }
    }
}
