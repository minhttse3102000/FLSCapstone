using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.RoomSemesterRequest;
using BEAPICapstoneProjectFLS.ViewModel;

namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class RoomSemesterMapper : Profile
    {
        public RoomSemesterMapper()
        {
            CreateMap<RoomSemester, RoomSemesterViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RoomSemesterStatus.Active))
                .ForMember(des => des.Term, s => s.MapFrom(s => s.Semester.Term))
                .ForMember(des => des.RoomTypeNameT, s => s.MapFrom(s => s.RoomType.RoomTypeName));

            CreateMap<RoomSemesterViewModel, RoomSemester>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RoomSemesterStatus.Active));

            CreateMap<CreateRoomSemesterRequest, RoomSemester>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RoomSemesterStatus.Active));

            CreateMap<UpdateRoomSemesterRequest, RoomSemester>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RoomSemesterStatus.Active));
        }
    }
}
