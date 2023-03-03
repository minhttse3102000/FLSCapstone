using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.LecturerCourseGroupRequest;
using BEAPICapstoneProjectFLS.ViewModel;

namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class LecturerCourseGroupMapper : Profile
    {
        public LecturerCourseGroupMapper()
        {
            CreateMap<LecturerCourseGroup, LecturerCourseGroupViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)LecturerCourseGroupStatus.Active))
                .ForMember(des => des.LecturerName, s => s.MapFrom(s => s.Lecturer.Name))
                .ForMember(des => des.Term, s => s.MapFrom(s => s.Semester.Term));

            CreateMap<LecturerCourseGroupViewModel, LecturerCourseGroup>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)LecturerCourseGroupStatus.Active));

            CreateMap<CreateLecturerCourseGroupRequest, LecturerCourseGroup>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)LecturerCourseGroupStatus.Active));

            CreateMap<UpdateLecturerCourseGroupRequest, LecturerCourseGroup>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)LecturerCourseGroupStatus.Active));
        }
    }
}
