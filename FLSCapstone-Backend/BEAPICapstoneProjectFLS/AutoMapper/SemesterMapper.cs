using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.Requests.SemesterRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using System;
using System.Linq;

namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class SemesterMapper: Profile
    {
        public SemesterMapper()
        {
            CreateMap<Semester, SemesterViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)SemesterStatus.Active))
                .ForMember(d => d.DateStartFormat, s => s.MapFrom(s => ConvertDateTimeToString(s.DateStart)))
                .ForMember(d => d.DateEndFormat, s => s.MapFrom(s => ConvertDateTimeToString(s.DateEnd)))
                .ForMember(d => d.DateStatus, s => s.MapFrom(s => ConvertDateStatus(s.DateStart, s.DateEnd)));

            CreateMap<SemesterViewModel, Semester>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)SemesterStatus.Active));
            CreateMap<CreateSemesterRequest, Semester>().ForMember(des
                => des.Status, opt => opt.MapFrom(src => (int)SemesterStatus.Active));
            CreateMap<UpdateSemesterRequest, Semester>().ForMember(des
                => des.Status, opt => opt.MapFrom(src => (int)SemesterStatus.Active));
        }

        private static string ConvertDateStatus(DateTime? dateStart, DateTime? dateEnd)
        {
            if (dateStart == null || dateEnd == null)
            {
                return null;
            }
            DateTime now = DateTime.Now;
            DateTime ds = Convert.ToDateTime(dateStart);
            DateTime de = Convert.ToDateTime(dateEnd); ;
        
            if (dateStart.HasValue && dateEnd.HasValue)
            {
                int compare = DateTime.Compare(ds, de);
                if (de <= ds)
                {
                    return "Wrong Data";
                }
                else
                {
                    if (now < ds)
                    {
                        return "Not Yet";
                    }
                    else if (now >= ds && now <= de)
                    {
                        return "On Going";
                    }
                    else
                    {
                        return "Close";
                    }
                }
            }
            return null;
        }

        private static string ConvertDateTimeToString(DateTime? date)
        {
            if (date != null) return date?.ToString("yyyy-MM-dd");
            return null;
        }
    }
}
