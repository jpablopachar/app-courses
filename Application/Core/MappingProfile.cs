using Application.Courses;
using Application.Instructors.GetInstructors;
using Application.Photos;
using Application.Prices.GetPrices;
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
            CreateMap<Course, CourseResponse>();
            CreateMap<Photo, PhotoResponse>();
            CreateMap<Price, PriceResponse>();

            // Mapea Instructor a InstructorResponse, asignando el apellido del instructor.
            CreateMap<Instructor, InstructorResponse>().ForMember(
                dest => dest.LastName,
                opt => opt.MapFrom(src => src.LastName)
            );

            // Mapea Qualification a QualificationResponse, asignando el nombre del curso.
            CreateMap<Qualification, QualificationResponse>().ForMember(
                dest => dest.CourseName,
                opt => opt.MapFrom(src => src.Course!.Title)
            );
        }
    }
}