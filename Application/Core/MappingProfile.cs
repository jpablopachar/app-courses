using Application.Qualifications.GetQualifications;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Qualification, QualificationResponse>().ForMember(
                dest => dest.CourseName,
                opt => opt.MapFrom(src => src.Course!.Title)
            );
        }
    }
}