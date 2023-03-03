using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.UserRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using System;
using System.Linq;

namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(d => d.Gender, s => s.MapFrom(s => MappingGender(s.Gender)))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)UserStatus.Active))
                .ForMember(d => d.Dob, s => s.MapFrom(s => s.Dob))
                .ForMember(d => d.DateOfBirthFormatted, s => s.MapFrom(s => ConvertDateTimeToString(s.Dob)))
                .ForMember(des => des.DepartmentName, s => s.MapFrom(s => s.Department.DepartmentName))
                .ForMember(des => des.RoleIDs, opt => opt.MapFrom(
                    src => src.UserAndRoles.Where(x => x.Status == (int)UserAndRoleStatus.Active)
                    .Select(x => x.RoleId)
                    ));

            //.ForMember(d => d.Dob, s => s.MapFrom(s => ConvertDateTimeToString(s.Dob)))
            //  .ForMember(d => d.Dob, s => s.MapFrom(s => s.Dob));
            //.ForMember(des => des.DepartmentName, s => s.MapFrom(s => s.Department.DepartmentName));

            CreateMap<UserViewModel, User>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)DepartmentStatus.Active))
                .ForMember(d => d.Gender, s => s.MapFrom(s => (int)s.Gender));

            CreateMap<CreateUserRequest, User>().ForMember(des
                => des.Status, opt => opt.MapFrom(src => (int)UserStatus.Active))
                .ForMember(d => d.Gender, s => s.MapFrom(s => (int)s.Gender));
            CreateMap<UpdateUserRequest, User>().ForMember(des
                => des.Status, opt => opt.MapFrom(src => (int)UserStatus.Active))
                .ForMember(d => d.Gender, s => s.MapFrom(s => (int)s.Gender));
        }

        private static Gender MappingGender(int? i)
        {
            if (i == 2) return Gender.Male;
            else if (i == 1) return Gender.Female;
            return Gender.Unknown;
        }

        private static string ConvertDateTimeToString(DateTime? date)
        {
            if (date != null) return date?.ToString("yyyy-MM-dd");
            return null;
        }
    }
}
