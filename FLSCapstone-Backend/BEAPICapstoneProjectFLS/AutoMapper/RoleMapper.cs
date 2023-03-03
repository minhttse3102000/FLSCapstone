using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.RoleRequest;
using BEAPICapstoneProjectFLS.ViewModel;
namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            CreateMap<Role, RoleViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RoleStatus.Active));

            CreateMap<RoleViewModel, Role>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RoleStatus.Active));

            CreateMap<CreateRoleRequest, Role>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RoleStatus.Active));

            CreateMap<UpdateRoleRequest, Role>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RoleStatus.Active));
        }
    }
}
