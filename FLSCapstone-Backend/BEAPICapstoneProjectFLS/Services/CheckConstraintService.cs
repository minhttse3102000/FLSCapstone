using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.IRepositories;
using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PagedList;
using Reso.Core.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.RandomKey;
using BEAPICapstoneProjectFLS.Requests.CourseAssignRequest;
using System;
using BEAPICapstoneProjectFLS.Requests.CourseRequest;

namespace BEAPICapstoneProjectFLS.Services
{
    public class CheckConstraintService : ICheckConstraintService
    {
        private readonly IGenericRepository<CourseAssign> _resCourseAssign;
        private readonly IGenericRepository<Course> _resCourse;
        private readonly IGenericRepository<User> _resUser;
        private readonly IGenericRepository<LecturerCourseGroup> _resLecturerCourseGroup;
        private readonly IGenericRepository<Schedule> _resSchedule;
        private readonly IMapper _mapper;


        public CheckConstraintService(
                    IGenericRepository<CourseAssign> courseAssignRepository, IGenericRepository<Course> courseRepository,
                    IGenericRepository<User> userRepository,
                    IGenericRepository<LecturerCourseGroup> lecturerCourseGroupRepository,
                    IGenericRepository<Schedule> scheduleRepository,
                    IMapper mapper)
        {
            _resCourseAssign = courseAssignRepository;
            _resCourse = courseRepository;
            _resUser = userRepository;
            _resLecturerCourseGroup = lecturerCourseGroupRepository;
            _resSchedule = scheduleRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CheckSemesterPublic(string semesterID)
        {
            ApiResponse apiResponse = new ApiResponse();
            apiResponse.Success = false;
            apiResponse.Message = "Check Semester Public fail";

            try
            {
                var listCourse = await _resCourse.GetAllByIQueryable()
                                    .Where(x => x.Status == (int)CourseAssignStatus.Active && x.SemesterId == semesterID)
                                    .ToListAsync();

                var schedule = await _resSchedule.GetAllByIQueryable()
                                    .Where(x => x.Status == (int)ScheduleStatus.Active && x.SemesterId == semesterID)
                                    .FirstOrDefaultAsync();

                var listCourseAssign = await _resCourseAssign.GetAllByIQueryable()
                                    .Where(x => x.Status == (int)CourseAssignStatus.Active && x.ScheduleId == schedule.Id)
                                    .ToListAsync();

                if(listCourse != null && listCourseAssign != null && listCourseAssign.Count > 0)
                {
                    apiResponse.Data = listCourseAssign.Count + " CourseAssigns / total " + listCourse.Count + " Courses";
                    if (listCourse.Count == listCourseAssign.Count)
                    {
                        var listUser = _resUser.FindBy(x => x.Status == (int)FLSStatus.Active);
                        UserViewModel flitter = new UserViewModel { RoleIDs = new List<string>() { "LC" } };
                        var listLecturer = await (listUser.ProjectTo<UserViewModel>
                            (_mapper.ConfigurationProvider)).DynamicFilter(flitter).ToListAsync();

                        foreach (var lecturer in listLecturer)
                        {
                            var rs = await CheckCourseOflecrurer(lecturer.Id, semesterID);

                            if (rs.Success == false)
                            {
                                apiResponse.Data = rs.Data;
                                return apiResponse;
                            }
                        }
                        apiResponse.Success = true;
                        apiResponse.Message = "Check Semester Public Success";

                        return apiResponse;
                    }
                    return apiResponse;

                }
                apiResponse.Data = "List course assign null";
                return apiResponse;


            }
            catch (Exception ex)
            {
                apiResponse.Data = ex.Message;
                return apiResponse;
            }
        }

        public async Task<List<UserViewModel>> GetListDepartmentManager()
        {
            var listUser = _resUser.FindBy(x => x.Status == (int)FLSStatus.Active);
            UserViewModel flitter = new UserViewModel { RoleIDs = new List<string>() { "DMA" } };
            var listDepartmentManager = await (listUser.ProjectTo<UserViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter).ToListAsync();
            return listDepartmentManager;
        }
        public async Task<ApiResponse> CheckAllDepartmentManagerConfirm(string semesterID)
        {
            ApiResponse apiResponse = new ApiResponse();
            apiResponse.Success = false;
            apiResponse.Message = "Check All Department Manager Confirm fail";
            try
            {

                var listDepartmentManager = await GetListDepartmentManager();

                if (listDepartmentManager!= null)
                {
                    List<string> departmentManagerNames = new List<string>();
                    foreach (var dma in listDepartmentManager)
                    {
                        var lecturerCourseGroup = await _resLecturerCourseGroup.GetAllByIQueryable()
                                                    .Where(x => x.LecturerId == dma.Id && x.SemesterId == semesterID && x.Status == (int)LecturerCourseGroupStatus.Active)
                                                    .FirstOrDefaultAsync();
                        if (lecturerCourseGroup.GroupName != "confirm")
                        {
                            string nameDMAnotConfirm = "Department Manager " + dma.Name + " hasn't confirmed";
                            departmentManagerNames.Add(nameDMAnotConfirm);                         
                        }

                    }

                    if(departmentManagerNames.Count > 0)
                    {
                        apiResponse.Data = departmentManagerNames;
                        return apiResponse;
                    }
                    apiResponse.Success = true;
                    apiResponse.Message = "Check All Department Manager Confirm Success";
                    return apiResponse;



                }
                apiResponse.Data = "listDepartmentManager null";
                return apiResponse;

            }
            catch(Exception ex)
            {
                apiResponse.Data=ex.Message;    
                return apiResponse;
            }
        }

        public async Task<ApiResponse> SetAllDepartmentManagerConfirm(string semesterID)
        {
            ApiResponse apiResponse = new ApiResponse();
            apiResponse.Success = false;
            apiResponse.Message = "Set All Department Manager Confirm fail";
            try
            {

                var listDepartmentManager = await GetListDepartmentManager();

                if (listDepartmentManager != null)
                {
                    foreach (var dma in listDepartmentManager)
                    {
                        var lecturerCourseGroup = await _resLecturerCourseGroup.GetAllByIQueryable()
                                                    .Where(x => x.LecturerId == dma.Id && x.SemesterId == semesterID && x.Status == (int)LecturerCourseGroupStatus.Active)
                                                    .FirstOrDefaultAsync();
                        lecturerCourseGroup.GroupName = "confirm";
                        await _resLecturerCourseGroup.UpdateAsync(lecturerCourseGroup);
                        await _resLecturerCourseGroup.SaveAsync();

                    }
                    apiResponse.Success = true;
                    apiResponse.Message = "Set All Department Manager Confirm Success";
                    return apiResponse;

                }
                apiResponse.Data = "listDepartmentManager null";
                return apiResponse;

            }
            catch (Exception ex)
            {
                apiResponse.Data = ex.Message;
                return apiResponse;
            }
        }

        public async Task<CourseOfLecturer> GetCourseOfLecturer(string lecturerID, string semesterID)
        {
            CourseOfLecturer courseOfLecturer;
            try
            {
                var schedule = await _resSchedule.GetAllByIQueryable()
                                    .Where(x => x.Status == (int)ScheduleStatus.Active && x.SemesterId == semesterID)
                                    .FirstOrDefaultAsync();
                string scheduleID = schedule.Id;


                var listCourseAssign = await _resCourseAssign.GetAllByIQueryable()
                                    .Where(x => x.Status == (int)CourseAssignStatus.Active && x.LecturerId == lecturerID && x.ScheduleId == scheduleID)
                                    .ToListAsync();

                var lecturerCourseGroup = await _resLecturerCourseGroup.GetAllByIQueryable()
                                    .Where(x => x.Status == (int)ScheduleStatus.Active && x.LecturerId == lecturerID && x.SemesterId == semesterID)
                                    .FirstOrDefaultAsync();
                if(lecturerCourseGroup.MinCourse.HasValue && lecturerCourseGroup.MaxCourse.HasValue)
                {
                    courseOfLecturer = new CourseOfLecturer();
                    courseOfLecturer.LecturerID = lecturerCourseGroup.LecturerId;
                    courseOfLecturer.AvailableCourse = listCourseAssign.Count;
                    courseOfLecturer.MinCourse = (int)lecturerCourseGroup.MinCourse;
                    courseOfLecturer.MaxCourse = (int)lecturerCourseGroup.MaxCourse;                  
                    if (courseOfLecturer.AvailableCourse >= courseOfLecturer.MinCourse && courseOfLecturer.AvailableCourse <= courseOfLecturer.MaxCourse)
                    {
                        return courseOfLecturer;
                    }
                    return courseOfLecturer;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ApiResponse> CheckCourseOflecrurer(string lecturerID, string semesterID)
        {
            ApiResponse apiResponse = new ApiResponse();
            apiResponse.Success = false;
            apiResponse.Message = "Check CourseOflecrurer fail";
            try
            {
                var courseOfLecturer = await GetCourseOfLecturer(lecturerID, semesterID);

                if(courseOfLecturer == null)
                {
                    apiResponse.Data = "CourseOflecrurer null";
                    return apiResponse;
                }

                if (courseOfLecturer.AvailableCourse >= courseOfLecturer.MinCourse && courseOfLecturer.AvailableCourse <= courseOfLecturer.MaxCourse)
                {
                    apiResponse.Success = true;
                    apiResponse.Message = "Check CourseOflecrurer Success";
                    apiResponse.Data = courseOfLecturer;
                    return apiResponse;
                }

                apiResponse.Data = courseOfLecturer;
                return apiResponse;

            }
            catch(Exception ex)
            {
                apiResponse.Data = ex.Message;
                return apiResponse;
            }
        }

        
    }
}
