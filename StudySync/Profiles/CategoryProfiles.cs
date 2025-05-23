using AutoMapper;
using StudySync.DTOs;
using StudySync.Models;

namespace StudySync.Profiles
{
    public class CategoryProfiles : Profile
    {
       public CategoryProfiles() 
        {
            CreateMap<Category,CategoryDTO>();
            CreateMap<CategoryCreateDTO,Category>();
            CreateMap<CategoryUpdateDTO, Category>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
