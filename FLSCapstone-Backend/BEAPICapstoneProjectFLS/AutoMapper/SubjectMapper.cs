using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.SubjectRequest;
using BEAPICapstoneProjectFLS.ViewModel;
namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class SubjectMapper : Profile
    {
        public SubjectMapper()
        {
            CreateMap<Subject, SubjectViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)SubjectStatus.Active))
                .ForMember(des => des.DepartmentName, s => s.MapFrom(s => s.Department.DepartmentName));

            CreateMap<SubjectViewModel, Subject>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)SubjectStatus.Active));

            CreateMap<CreateSubjectRequest, Subject>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)SubjectStatus.Active));

            CreateMap<UpdateSubjectRequest, Subject>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)SubjectStatus.Active));
        }
    }
}
