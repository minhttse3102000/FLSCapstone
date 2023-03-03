using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.DepartmentRequest;
using BEAPICapstoneProjectFLS.ViewModel;

namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class DepartmentMapper : Profile
    {
        public DepartmentMapper()
        {
            CreateMap<Department, DepartmentViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)DepartmentStatus.Active))
                .ForMember(des => des.DepartmentGroupName, s => s.MapFrom(s => s.DepartmentGroup.DepartmentGroupName));

            CreateMap<DepartmentViewModel, Department>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)DepartmentStatus.Active));

            CreateMap<CreateDepartmentRequest, Department>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)DepartmentStatus.Active));

            CreateMap<UpdateDepartmentRequest, Department>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)DepartmentStatus.Active));
        }
    }
}
