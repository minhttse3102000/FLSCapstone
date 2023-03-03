using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using System.Linq;

namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class DepartmentGroupMapper : Profile
    {
        public DepartmentGroupMapper()
        {
            CreateMap<DepartmentGroup, DepartmentGroupViewModel>()
                .ForMember(d => d.DepartmentGroupName, s => s.MapFrom(s => s.DepartmentGroupName))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)DepartmentGroupStatus.Active))
                .ForMember(des => des.DepartmentIds, opt => opt.MapFrom(
                    src => src.Departments.Where(x => x.Status == (int)DepartmentGroupStatus.Active)
                    .Select(x => x.Id )
                    ));
            CreateMap<DepartmentGroupViewModel, DepartmentGroup>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)DepartmentGroupStatus.Active));
            CreateMap<CreateDepartmentGroupRequest, DepartmentGroup>().ForMember(des
                => des.Status, opt => opt.MapFrom(src => (int)DepartmentGroupStatus.Active));
            CreateMap<UpdateDepartmentGroupRequest, DepartmentGroup>().ForMember(des
                => des.Status, opt => opt.MapFrom(src => (int)DepartmentGroupStatus.Active));
        }

    }
}
