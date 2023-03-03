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
using BEAPICapstoneProjectFLS.Requests.CourseRequest;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BEAPICapstoneProjectFLS.Services
{
    public class CourseService : ICourseService
    {
        private readonly IGenericRepository<Course> _res;
        private readonly IMapper _mapper;

        public CourseService(IGenericRepository<Course> repository, IMapper mapper)
        {
            _res = repository;
            _mapper = mapper;
        }

        public async Task<CourseViewModel> CreateCourse(CreateCourseRequest request)
        {
            try
            {
                var cr = _mapper.Map<Course>(request);
                await _res.InsertAsync(cr);
                await _res.SaveAsync();

                var crVM = await GetCourseById(cr.Id);
                return crVM;
            }
            catch
            {
                return null;
            }

        }

 

        public async Task<ApiResponse> CreateListCourse(string semesterID, List<CreateCourseInSemesterRequest> requests)
        {
            try
            {
                foreach (var cr in requests)
                {
                    var newCourse = _mapper.Map<Course>(cr);
                    newCourse.SemesterId = semesterID;
                    await _res.InsertAsync(newCourse);
                    await _res.SaveAsync();
                }
                return new ApiResponse
                {
                    Success = true,
                    Message = "CreateListCourse success"
                };

            }
            catch(Exception e)
            {
                return new ApiResponse {
                    Success= false,
                    Message= "CreateListCourse fail",
                    Data = e.Message
                };
            }
        }

        public async Task<bool> DeleteCourse(string id)
        {
            var course = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)CourseStatus.Active)).FirstOrDefault();
            if (course == null)
            {
                return false;
            }
            course.Status = 0;
            await _res.UpdateAsync(course);
            await _res.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteListCourseInSemester(string SemesterID)
        {
            try
            {
                var listCourse = await _res.GetAllByIQueryable().Where(x => x.Status == (int)CourseStatus.Active)
                    .Where(x => x.SemesterId == SemesterID)
                    .ToListAsync();
                if(listCourse.Count <= 0 )
                {
                    return false;
                }
                else
                {
                    foreach (var course in listCourse)
                    {
                        await _res.DeleteAsync(course.Id);
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


        public IPagedList<CourseViewModel> GetAllCourse(CourseViewModel flitter, int pageIndex, int pageSize, CourseSortBy sortBy, OrderBy order)
        {
            var listCourse = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

            var listCourseViewModel = (listCourse.ProjectTo<CourseViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "SubjectId":
                    if (order.ToString() == "Asc")
                    {
                        listCourseViewModel = (IQueryable<CourseViewModel>)listCourseViewModel.OrderBy(x => x.SubjectId);
                    }
                    else
                    {
                        listCourseViewModel = (IQueryable<CourseViewModel>)listCourseViewModel.OrderByDescending(x => x.SubjectId);
                    }
                    break;
                case "Id":
                    if (order.ToString() == "Asc")
                    {
                        listCourseViewModel = (IQueryable<CourseViewModel>)listCourseViewModel.OrderBy(x => x.Id);
                    }
                    else
                    {
                        listCourseViewModel = (IQueryable<CourseViewModel>)listCourseViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<CourseViewModel>(listCourseViewModel, pageIndex, pageSize);
        }


        public async Task<CourseViewModel> GetCourseById(string id)
        {
            var dg = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)CourseStatus.Active)
                .Include(x => x.Subject)
                .Include(x => x.Semester)
                .FirstOrDefaultAsync();
            if (dg == null)
                return null;
            var CourseVM = _mapper.Map<CourseViewModel>(dg);
            return CourseVM;
        }

        public async Task<CourseViewModel> UpdateCourse(string id, UpdateCourseRequest request)
        {
            try
            {
                var listCourse = await _res.FindByAsync(x => x.Id == id && x.Status == (int)CourseStatus.Active);
                if (listCourse == null)
                {
                    return null;
                }
                var course = listCourse.FirstOrDefault();
                if (course == null)
                {
                    return null;
                }
                course = _mapper.Map(request, course);
                await _res.UpdateAsync(course);
                await _res.SaveAsync();

                var crVM = await GetCourseById(course.Id);
                return crVM;
            }
            catch
            {
                return null;
            }
        }


    }
}
