using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.CourseGroupItemRequest;
using BEAPICapstoneProjectFLS.ViewModel;

namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class CourseGroupItemMapper : Profile
    {
        public CourseGroupItemMapper()
        {
            CreateMap<CourseGroupItem, CourseGroupItemViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)CourseGroupItemStatus.Active))
                .ForMember(des => des.GroupName, s => s.MapFrom(s => s.LecturerCourseGroup.GroupName));

            CreateMap<CourseGroupItemViewModel, CourseGroupItem>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)CourseGroupItemStatus.Active));

            CreateMap<CreateCourseGroupItemRequest, CourseGroupItem>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)CourseGroupItemStatus.Active));

            CreateMap<UpdateCourseGroupItemRequest, CourseGroupItem>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)CourseGroupItemStatus.Active));
        }
    }
}
