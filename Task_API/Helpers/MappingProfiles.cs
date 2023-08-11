using AutoMapper;
using Task_API.Models;
using Task_API.Models.Dto;

namespace Task_API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<TaskDo, TaskDto>()
                .ForMember(x => x.Status, o => o.MapFrom(x => x.Status.Name));

            CreateMap<TaskCreateDto, TaskDo>();
            CreateMap<Status, StatusDto>();
        }
    }
}
