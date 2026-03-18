using Application.Qualifications.GetQualifications;
using AutoMapper;
using Domain;

namespace Application.Core
{
    /// <summary>
    /// Perfil de mapeo para AutoMapper, define las conversiones entre entidades y modelos de respuesta.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapea Qualification a QualificationResponse, asignando el nombre del curso.
            CreateMap<Qualification, QualificationResponse>().ForMember(
                dest => dest.CourseName,
                opt => opt.MapFrom(src => src.Course!.Title)
            );
        }
    }
}