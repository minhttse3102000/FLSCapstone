using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.RoomTypeRequest;
using BEAPICapstoneProjectFLS.ViewModel;

namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class RoomTypeMapper : Profile
    {
        public RoomTypeMapper()
        {
            CreateMap<RoomType, RoomTypeViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RoomTypeStatus.Active));

            CreateMap<RoomTypeViewModel, RoomType>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RoomTypeStatus.Active));

            CreateMap<CreateRoomTypeRequest, RoomType>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RoomTypeStatus.Active));

            CreateMap<UpdateRoomTypeRequest, RoomType>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RoomTypeStatus.Active));
        }
    }
}
