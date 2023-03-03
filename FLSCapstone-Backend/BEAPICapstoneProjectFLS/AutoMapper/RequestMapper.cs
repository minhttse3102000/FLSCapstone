using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.Request;
using BEAPICapstoneProjectFLS.ViewModel;
using System;

namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class RequestMapper : Profile
    {
        public RequestMapper()
        {
            CreateMap<Request, RequestViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RequestStatus.Active))             
                .ForMember(des => des.DepartmentManagerName, s => s.MapFrom(s => s.DepartmentManager.Name))
                .ForMember(des => des.LecturerName, s => s.MapFrom(s => s.Lecturer.Name))
                .ForMember(des => des.Term, s => s.MapFrom(s => s.Semester.Term))
                .ForMember(des => des.SubjectName, s => s.MapFrom(s => s.Subject.SubjectName))
                .ForMember(d => d.DateCreateFormat, s => s.MapFrom(s => ConvertDateTimeToString(s.DateCreate)))
                .ForMember(d => d.DateResponeFormat, s => s.MapFrom(s => ConvertDateTimeToString(s.DateRespone)));

            CreateMap<RequestViewModel, Request>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RequestStatus.Active));

            CreateMap<CreateRequest, Request>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RequestStatus.Active));

            CreateMap<UpdateRequest, Request>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)RequestStatus.Active));
        }

        private static string ConvertDateTimeToString(DateTime? date)
        {
            if (date != null) return date?.ToString("yyyy-MM-dd HH:mm:ss");
            return null;
        }
    }
}
