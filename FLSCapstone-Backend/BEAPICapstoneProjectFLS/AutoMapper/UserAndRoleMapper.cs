using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.UserAndRoleRequest;
using BEAPICapstoneProjectFLS.ViewModel;
namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class UserAndRoleMapper : Profile
    {
        public UserAndRoleMapper()
        {
            CreateMap<UserAndRole, UserAndRoleViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)UserAndRoleStatus.Active))
                .ForMember(des => des.RoleName, s => s.MapFrom(s => s.Role.RoleName));

            CreateMap<UserAndRoleViewModel, UserAndRole>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)UserAndRoleStatus.Active));

            CreateMap<CreateUserAndRoleRequest, UserAndRole>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)UserAndRoleStatus.Active));

            CreateMap<UpdateUserAndRoleRequest, UserAndRole>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)UserAndRoleStatus.Active));
        }
    }
}
