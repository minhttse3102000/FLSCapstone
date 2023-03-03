using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.LecturerSlotConfigRequest;
using BEAPICapstoneProjectFLS.ViewModel;

namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class LecturerSlotConfigMapper : Profile
    {
        public LecturerSlotConfigMapper()
        {
            CreateMap<LecturerSlotConfig, LecturerSlotConfigViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)LecturerSlotConfigStatus.Active))
                .ForMember(des => des.LecturerName, s => s.MapFrom(s => s.Lecturer.Name))
                .ForMember(des => des.SlotTypeCode, s => s.MapFrom(s => s.SlotType.SlotTypeCode))
                .ForMember(des => des.Term, s => s.MapFrom(s => s.Semester.Term));

            CreateMap<LecturerSlotConfigViewModel, LecturerSlotConfig>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)LecturerSlotConfigStatus.Active));

            CreateMap<CreateLecturerSlotConfigRequest, LecturerSlotConfig>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)LecturerSlotConfigStatus.Active));

            CreateMap<UpdateLecturerSlotConfigRequest, LecturerSlotConfig>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)LecturerSlotConfigStatus.Active));
        }
    }
}
