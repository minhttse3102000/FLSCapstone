using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.ScheduleRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using System;

namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class ScheduleMapper : Profile
    {
        public ScheduleMapper()
        {
            CreateMap<Schedule, ScheduleViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)ScheduleStatus.Active))
                .ForMember(des => des.Term, s => s.MapFrom(s => s.Semester.Term))
                 .ForMember(d => d.FormatDateCreate, s => s.MapFrom(s => ConvertDateTimeToString(s.DateCreate)));

            CreateMap<ScheduleViewModel, Schedule>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)ScheduleStatus.Active));

            CreateMap<CreateScheduleRequest, Schedule>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)ScheduleStatus.Active));

            CreateMap<UpdateScheduleRequest, Schedule>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)ScheduleStatus.Active));
        }

        private static string ConvertDateTimeToString(DateTime? date)
        {
            if (date != null) return date?.ToString("yyyy-MM-dd HH:mm:ss");
            return null;
        }
    }
}
