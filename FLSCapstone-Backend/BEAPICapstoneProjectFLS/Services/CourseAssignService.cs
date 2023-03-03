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

namespace BEAPICapstoneProjectFLS.Services
{
    public class CourseAssignService : ICourseAssignService
    {
        private readonly IGenericRepository<CourseAssign> _res;
        private readonly IGenericRepository<Course> _resCourse;
        private readonly IGenericRepository<User> _resUser;
        private readonly IGenericRepository<Subject> _resSubject;
        private readonly IMapper _mapper;

        public CourseAssignService(IGenericRepository<CourseAssign> repository, IGenericRepository<Course> courseRepository,
                            IGenericRepository<User> userRepository, IGenericRepository<Subject> subjectRepository,
                            IMapper mapper)
        {
            _res = repository;
            _resCourse = courseRepository;
            _resUser = userRepository;
            _resSubject = subjectRepository;
            _mapper = mapper;
        }

        public async Task<CourseAssignViewModel> CreateCourseAssign(CreateCourseAssignRequest request)
        {
            try
            {
                var ca = _mapper.Map<CourseAssign>(request);
                ca.Id = RandomPKKey.NewRamDomPKKey();
                await _res.InsertAsync(ca);
                await _res.SaveAsync();

                var caVM = await GetCourseAssignById(ca.Id);
                return caVM;
            }
            catch
            {
                return null;
            }

        }

        public bool checkScheduleID(string ScheduleID, List<CreateCourseAssignRequest> requests)
        {
            foreach (var checkSchedule in requests)
            {
                if (checkSchedule.ScheduleId != ScheduleID)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> CreateListCourseAssign(string ScheduleID, List<CreateCourseAssignRequest> requests)
        {
            try
            {
                var check = checkScheduleID(ScheduleID, requests);
                if (check)
                {
                    foreach (var cr in requests)
                    {
                        var newCourseAssign = _mapper.Map<CourseAssign>(cr);
                        newCourseAssign.Id = RandomPKKey.NewRamDomPKKey();
                        await _res.InsertAsync(newCourseAssign);
                        await _res.SaveAsync();
                    }
                    return true;
                }
                return false;

            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCourseAssign(string id)
        {
            var courseAssign = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)CourseAssignStatus.Active)).FirstOrDefault();
            if (courseAssign == null)
            {
                return false;
            }
            courseAssign.Status = 0;
            await _res.UpdateAsync(courseAssign);
            await _res.SaveAsync();
            return true;
        }


        public async Task<bool> DeleteListCourseAssignInSemester(string ScheduleID)
        {
            try
            {
                var listCourseAssign = await _res.GetAllByIQueryable().Where(x => x.Status == (int)CourseAssignStatus.Active)
                    .Where(x => x.ScheduleId == ScheduleID)
                    .Where(x => x.IsAssign == 0)
                    .ToListAsync();
                if (listCourseAssign.Count <= 0)
                {
                    return false;
                }
                else
                {
                    foreach (var CourseAssign in listCourseAssign)
                    {
                        await _res.DeleteAsync(CourseAssign.Id);
                    }
                    return true;
                }

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
        }

        public async Task<bool> DeleteAssignedCourses(string ScheduleID)
        {
            try
            {
                var listCourseAssign = await _res.GetAllByIQueryable().Where(x => x.Status == (int)CourseAssignStatus.Active)
                    .Where(x => x.ScheduleId == ScheduleID)
                    .Where(x => x.IsAssign == 1)
                    .ToListAsync();
                if (listCourseAssign.Count <= 0)
                {
                    return false;
                }
                else
                {
                    foreach (var CourseAssign in listCourseAssign)
                    {
                        await _res.DeleteAsync(CourseAssign.Id);
                    }
                    return true;
                }

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
        }

        public IPagedList<CourseAssignViewModel> GetAllCourseAssign(CourseAssignViewModel flitter, int pageIndex, int pageSize, CourseAssignSortBy sortBy, OrderBy order)
        {
            var listCourseAssign = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

            var listCourseAssignViewModel = (listCourseAssign.ProjectTo<CourseAssignViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "Id":
                    if (order.ToString() == "Asc")
                    {
                        listCourseAssignViewModel = (IQueryable<CourseAssignViewModel>)listCourseAssignViewModel.OrderBy(x => x.Id);
                    }
                    else
                    {
                        listCourseAssignViewModel = (IQueryable<CourseAssignViewModel>)listCourseAssignViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
                case "LecturerId":
                    if (order.ToString() == "Asc")
                    {
                        listCourseAssignViewModel = (IQueryable<CourseAssignViewModel>)listCourseAssignViewModel.OrderBy(x => x.LecturerId);
                    }
                    else
                    {
                        listCourseAssignViewModel = (IQueryable<CourseAssignViewModel>)listCourseAssignViewModel.OrderByDescending(x => x.LecturerId);
                    }
                    break;
                case "CourseId":
                    if (order.ToString() == "Asc")
                    {
                        listCourseAssignViewModel = (IQueryable<CourseAssignViewModel>)listCourseAssignViewModel.OrderBy(x => x.CourseId);
                    }
                    else
                    {
                        listCourseAssignViewModel = (IQueryable<CourseAssignViewModel>)listCourseAssignViewModel.OrderByDescending(x => x.CourseId);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<CourseAssignViewModel>(listCourseAssignViewModel, pageIndex, pageSize);
        }


        public async Task<CourseAssignViewModel> GetCourseAssignById(string id)
        {


            var dg = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)CourseAssignStatus.Active)
                .Include(x => x.Lecturer)
                .Include(x => x.Course)
                .Include(x => x.SlotType)
                .FirstOrDefaultAsync();
            if (dg == null)
                return null;

            var courseAssignVM = _mapper.Map<CourseAssignViewModel>(dg);
            return courseAssignVM;
        }
        public async Task<IEnumerable<CourseAssignViewModel>> GetCourseAssignByGroup(string GroupID)
        {
            #region

            //var listCourseAssign = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

            //var listCourseAssignViewModel = await (listCourseAssign.ProjectTo<CourseAssignViewModel>
            //    (_mapper.ConfigurationProvider)).DynamicFilter(new CourseAssignViewModel { }).ToListAsync();

            //return listCourseAssignViewModel;



            //List<CourseAssign> result = new List<CourseAssign>();
            //try
            //{
            //    var courseAssigns = await _res.GetAllByIQueryable()
            //                 .Where(x => x.CourseId.Contains(GroupID) && x.Status == (int)CourseAssignStatus.Active)
            //                 .Include(x => x.Lecturer)
            //                 .Include(x => x.Course)
            //                 .ToListAsync();
            //    if (courseAssigns == null)
            //        return null;

            //    var courseAssignVM = _mapper.Map<IEnumerable<CourseAssignViewModel>>(result);
            //    return courseAssignVM;
            //}
            //catch (Exception ex)
            //{
            //    string error = ex.Message;
            //    return null;
            //}
            #endregion

            List<CourseAssign> result = new List<CourseAssign>();
            try
            {
                var courseAssigns = await _res.GetAllByIQueryable()
                             .Where(x => x.Status == (int)CourseAssignStatus.Active)
                             .Include(x => x.Lecturer)
                             .Include(x => x.Course)
                             .Include(x => x.SlotType)
                             .ToListAsync();
                if (courseAssigns == null)
                    return null;
                foreach (var ca in courseAssigns)
                {
                    string s = ca.CourseId.Split('_')[1];
                    if (s == GroupID)
                    {
                        result.Add(ca);
                    }
                }

                var courseAssignVM = _mapper.Map<IEnumerable<CourseAssignViewModel>>(result);
                return courseAssignVM;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }

        }

        public async Task<CourseAssignViewModel> UpdateCourseAssign(string id, UpdateCourseAssignRequest request)
        {
            try
            {
                var listCourseAssign = await _res.FindByAsync(x => x.Id == id && x.Status == (int)CourseAssignStatus.Active);
                if (listCourseAssign == null)
                {
                    return null;
                }
                var courseAssign = listCourseAssign.FirstOrDefault();
                if (courseAssign == null)
                {
                    return null;
                }
                courseAssign = _mapper.Map(request, courseAssign);
                await _res.UpdateAsync(courseAssign);
                await _res.SaveAsync();

                var caVM = await GetCourseAssignById(courseAssign.Id);
                return caVM;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> checkLecturerInOutDepartment(string subjectID, string departmentID)
        {
            var subject = await _resSubject.GetAllByIQueryable()
                 .Where(x => x.Id == subjectID && x.Status == (int)UserStatus.Active)
                 .FirstOrDefaultAsync();
            if (subject.DepartmentId == departmentID)
            {
                return true;
            }
            return false;
        }

        public async Task<List<User>> GetUserAssign(string subjectID, string semesterID, bool inDepartment)
        {
            List<User> result = new List<User>();
            try
            {
                var listCourse = await _resCourse.GetAllByIQueryable()
                                     .Where(x => x.SemesterId == semesterID && x.SubjectId == subjectID && x.Status == (int)CourseStatus.Active)
                                     .ToListAsync();

                var listCourseAssign = await _res.GetAllByIQueryable()
                                     .Where(x => x.Status == (int)CourseAssignStatus.Active)
                                     .ToListAsync();

                var listUserAssign = from course in listCourse
                                     join courseAssign in listCourseAssign on course.Id equals courseAssign.CourseId
                                     group course by courseAssign.LecturerId;

                if (inDepartment == true)
                {
                    foreach (var group in listUserAssign)
                    {
                        var user = await _resUser.GetAllByIQueryable()
                                         .Where(x => x.Id == group.Key && x.Status == (int)UserStatus.Active)
                                         .Include(x => x.Department)
                                         .Include(x => x.UserAndRoles)
                                         .FirstOrDefaultAsync();
                        bool checkInOutD = await checkLecturerInOutDepartment(subjectID, user.DepartmentId);
                        if (checkInOutD == true)
                        {
                            result.Add(user);
                        }
                    }

                    return result;
                }
                if (inDepartment == false)
                {
                    foreach (var group in listUserAssign)
                    {
                        var user = await _resUser.GetAllByIQueryable()
                                         .Where(x => x.Id == group.Key && x.Status == (int)UserStatus.Active)
                                         .Include(x => x.Department)
                                         .Include(x => x.UserAndRoles)
                                         .FirstOrDefaultAsync();

                        bool checkInOutD = await checkLecturerInOutDepartment(subjectID, user.DepartmentId);
                        if (checkInOutD == false)
                        {
                            result.Add(user);
                        }
                    }

                    return result;
                }
                return null;
            }
            catch
            {
                return null;
            }
            
        }

        public async Task<IEnumerable<UserViewModel>> GetUserAssignInDepartment(string subjectID, string semesterID)
        {
            List<User> rs = await GetUserAssign(subjectID, semesterID, true);
            return _mapper.Map<IEnumerable<UserViewModel>>(rs);
        }

        public async Task<IEnumerable<UserViewModel>> GetUserAssignOutDepartment(string subjectID, string semesterID)
        {
            List<User> rs = await GetUserAssign(subjectID, semesterID, false);
            return _mapper.Map<IEnumerable<UserViewModel>>(rs);
        }
    }
}
