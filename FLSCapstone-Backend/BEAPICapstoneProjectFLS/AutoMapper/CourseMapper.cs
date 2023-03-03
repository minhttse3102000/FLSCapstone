using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.CourseRequest;
using BEAPICapstoneProjectFLS.ViewModel;


namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class CourseMapper : Profile
    {
        public CourseMapper()
        {
            CreateMap<Course, CourseViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)CourseStatus.Active))
                .ForMember(des => des.SubjectName, s => s.MapFrom(s => s.Subject.SubjectName))
                .ForMember(des => des.Term, s => s.MapFrom(s => s.Semester.Term));

            CreateMap<CourseViewModel, Course>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)CourseStatus.Active));

            CreateMap<CreateCourseRequest, Course>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)CourseStatus.Active));

            CreateMap<CreateCourseInSemesterRequest, Course>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)CourseStatus.Active));

            CreateMap<UpdateCourseRequest, Course>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)CourseStatus.Active));
        }
    }
}
