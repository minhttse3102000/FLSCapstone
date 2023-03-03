using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests.CourseAssignRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using System;

namespace BEAPICapstoneProjectFLS.AutoMapper
{
    public class CourseAssignMapper : Profile
    {
        public CourseAssignMapper()
        {
            CreateMap<CourseAssign, CourseAssignViewModel>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)CourseAssignStatus.Active))
                .ForMember(des => des.SlotTypeCode, s => s.MapFrom(s => s.SlotType.SlotTypeCode))
                .ForMember(des => des.LecturerName, s => s.MapFrom(s => s.Lecturer.Name))
                .ForMember(d => d.Duration, s => s.MapFrom(s => ConvertTimeToDuration(s.SlotType.TimeStart, s.SlotType.TimeEnd)))
                .ForMember(d => d.ConvertDateOfWeek, s => s.MapFrom(s => ConvertDateOfWeek(s.SlotType.DateOfWeek)));

            CreateMap<CourseAssignViewModel, CourseAssign>()
                .ForMember(des => des.Status, opt => opt.MapFrom(src => (int)CourseAssignStatus.Active));

            CreateMap<CreateCourseAssignRequest, CourseAssign>().ForMember(des
                => des.Status, opt => opt.MapFrom(src => (int)CourseAssignStatus.Active));
            CreateMap<UpdateCourseAssignRequest, CourseAssign>().ForMember(des
                => des.Status, opt => opt.MapFrom(src => (int)CourseAssignStatus.Active));
        }


        private static string ConvertTimeToDuration(TimeSpan? timeStart, TimeSpan? timeEnd)
        {
            if (timeStart == null && timeEnd == null)
            {
                return null;
            }
            else
            {
                String result;
                result = timeStart?.ToString(@"hh\:mm") + " - " + timeEnd?.ToString(@"hh\:mm");
                return result;
            }
        }

        private static string ConvertDateOfWeek(int? dateOfWeek)
        {
            if (dateOfWeek == null)
            {
                return null;
            }
            else
            {
                string result = "";
                int n = (int)dateOfWeek;
                int countTwiceWeek = 0;
                for (int i = 0; n > 0; i++)
                {
                    if (n % 2 == 1)
                    {
                        countTwiceWeek++;
                        if (countTwiceWeek == 2)
                        {
                            result += ConvertNumToDate(i);
                        }
                        else
                        {
                            result += ConvertNumToDate(i) + " - ";
                        }
                    }

                    n = n / 2;
                }
                return result;
            }
        }

        private static string ConvertNumToDate(int num)
        {
            switch (num)
            {
                case 2:
                    return "Monday";
                case 3:
                    return "Tuesday";
                case 4:
                    return "Wednesday";
                case 5:
                    return "Thursday";
                case 6:
                    return "Friday";
                case 7:
                    return "Saturday";
                case 8:
                    return "Sunday";
                default:
                    return "";

            }
        }
    }
}
