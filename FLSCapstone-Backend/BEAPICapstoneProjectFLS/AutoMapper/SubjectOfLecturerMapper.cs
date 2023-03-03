using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.SubjectOfLecturerRequest;
using BEAPICapstoneProjectFLS.ViewModel;
namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class SubjectOfLecturerMapper : Profile
    {
        public SubjectOfLecturerMapper()
        {
            CreateMap<SubjectOfLecturer, SubjectOfLecturerViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)SubjectOfLecturerStatus.Active))
                .ForMember(des => des.LecturerName, s => s.MapFrom(s => s.Lecturer.Name))
                .ForMember(des => des.DepartmentManagerName, s => s.MapFrom(s => s.DepartmentManager.Name))
                .ForMember(des => des.Term, s => s.MapFrom(s => s.Semester.Term))
                .ForMember(des => des.SubjectName, s => s.MapFrom(s => s.Subject.SubjectName));

            CreateMap<SubjectOfLecturerViewModel, SubjectOfLecturer>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)SubjectOfLecturerStatus.Active));

            CreateMap<CreateSubjectOfLecturerRequest, SubjectOfLecturer>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)SubjectOfLecturerStatus.Active));

            CreateMap<UpdateSubjectOfLecturerRequest, SubjectOfLecturer>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)SubjectOfLecturerStatus.Active));
        }
    }
}
