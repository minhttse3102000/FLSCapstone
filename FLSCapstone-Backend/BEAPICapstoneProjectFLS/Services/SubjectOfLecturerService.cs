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
using BEAPICapstoneProjectFLS.Requests.SubjectOfLecturerRequest;
using System;

namespace BEAPICapstoneProjectFLS.Services
{
    public class SubjectOfLecturerService : ISubjectOfLecturerService
    {
        private readonly IGenericRepository<SubjectOfLecturer> _res;
        private readonly IGenericRepository<User> _resUser;
        private readonly IGenericRepository<Subject> _resSubject;
        private readonly IMapper _mapper;

        public SubjectOfLecturerService(IGenericRepository<SubjectOfLecturer> repository, IGenericRepository<User> userRepository, IGenericRepository<Subject> subjectRepository, IMapper mapper)
        {
            _res = repository;
            _resUser = userRepository;
            _resSubject = subjectRepository;
            _mapper = mapper;
        }

        public async Task<SubjectOfLecturerViewModel> CreateSubjectOfLecturer(CreateSubjectOfLecturerRequest request)
        {
            try
            {
                var sol = _mapper.Map<SubjectOfLecturer>(request);
                sol.Id = RandomPKKey.NewRamDomPKKey();
                await _res.InsertAsync(sol);
                await _res.SaveAsync();

                var solVM = await GetSubjectOfLecturerById(sol.Id);
                return solVM;
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> DeleteSubjectOfLecturer(string id)
        {
            var subjectOfLecturer = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)SubjectOfLecturerStatus.Active)).FirstOrDefault();
            if (subjectOfLecturer == null)
            {
                return false;
            }
            subjectOfLecturer.Status = 0;
            await _res.UpdateAsync(subjectOfLecturer);
            await _res.SaveAsync();
            return true;
        }

        public IPagedList<SubjectOfLecturerViewModel> GetAllSubjectOfLecturer(SubjectOfLecturerViewModel flitter, int pageIndex, int pageSize, SubjectOfLecturerSortBy sortBy, OrderBy order)
        {
            var listSubjectOfLecturer = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

            var listSubjectOfLecturerViewModel = (listSubjectOfLecturer.ProjectTo<SubjectOfLecturerViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "LecturerId":
                    if (order.ToString() == "Asc")
                    {
                        listSubjectOfLecturerViewModel = (IQueryable<SubjectOfLecturerViewModel>)listSubjectOfLecturerViewModel.OrderBy(x => x.LecturerId);
                    }
                    else
                    {
                        listSubjectOfLecturerViewModel = (IQueryable<SubjectOfLecturerViewModel>)listSubjectOfLecturerViewModel.OrderByDescending(x => x.LecturerId);
                    }
                    break;
                case "Id":
                    if (order.ToString() == "Asc")
                    {
                        listSubjectOfLecturerViewModel = (IQueryable<SubjectOfLecturerViewModel>)listSubjectOfLecturerViewModel.OrderBy(x => x.Id);
                    }
                    else
                    {
                        listSubjectOfLecturerViewModel = (IQueryable<SubjectOfLecturerViewModel>)listSubjectOfLecturerViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<SubjectOfLecturerViewModel>(listSubjectOfLecturerViewModel, pageIndex, pageSize);
        }


        public async Task<SubjectOfLecturerViewModel> GetSubjectOfLecturerById(string id)
        {

            var listSOL = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

            var listSOLViewModel = (listSOL.ProjectTo<SubjectOfLecturerViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(new SubjectOfLecturerViewModel { Id = id });

            var sol = await listSOLViewModel.FirstOrDefaultAsync();
            if (sol == null)
                return null;
            return sol;

            /*var sol = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)SubjectOfLecturerStatus.Active)
                .Include(x => x.Lecturer)
                .Include(x => x.Semester)
                .Include(x => x.Subject)
                .Include(x => x.DepartmentManager)
                .FirstOrDefaultAsync();
            if (sol == null)
                return null;
            var subjectOfLecturerVM = _mapper.Map<SubjectOfLecturerViewModel>(sol);
            return subjectOfLecturerVM;*/
        }

        public async Task<SubjectOfLecturerViewModel> UpdateSubjectOfLecturer(string id, UpdateSubjectOfLecturerRequest request)
        {
            try
            {
                var listSubjectOfLecturer = await _res.FindByAsync(x => x.Id == id && x.Status == (int)SubjectOfLecturerStatus.Active);
                if (listSubjectOfLecturer == null)
                {
                    return null;
                }
                var subjectOfLecturer = listSubjectOfLecturer.FirstOrDefault();
                if (subjectOfLecturer == null)
                {
                    return null;
                }
                subjectOfLecturer = _mapper.Map(request, subjectOfLecturer);
                await _res.UpdateAsync(subjectOfLecturer);
                await _res.SaveAsync();

                var solVM = await GetSubjectOfLecturerById(subjectOfLecturer.Id);
                return solVM;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ApiResponse> CreateSubjectOfLecturerInSemester(string semesterID)
        {
            try
            {
                var listUser = _resUser.FindBy(x => x.Status == (int)FLSStatus.Active);


                UserViewModel flitter = new UserViewModel { RoleIDs = new List<string>() { "LC" } };
                var listLecturer = await (listUser.ProjectTo<UserViewModel>
                    (_mapper.ConfigurationProvider)).DynamicFilter(flitter).ToListAsync();

                foreach (var lec in listLecturer)
                {
                    var ListSubjectOfLecturer = await _resSubject.GetAllByIQueryable()
                        .Where(x => x.DepartmentId == lec.DepartmentId && x.Status == (int)SubjectOfLecturerStatus.Active)
                        .Include(x => x.Department)
                        .ToListAsync();

             
                    UserViewModel flitterGetDepartmentManager = new UserViewModel {DepartmentId = lec.DepartmentId, RoleIDs = new List<string>() { "DMA" } };
                    var DepartmentManager = await (listUser.ProjectTo<UserViewModel>
                        (_mapper.ConfigurationProvider)).DynamicFilter(flitterGetDepartmentManager).FirstOrDefaultAsync();


                    foreach (var subject in ListSubjectOfLecturer)
                    {
                        SubjectOfLecturer subjectOfLecturer = new SubjectOfLecturer
                        {
                            Id = RandomPKKey.NewRamDomPKKey(),
                            SemesterId = semesterID,
                            SubjectId = subject.Id,
                            DepartmentManagerId = DepartmentManager.Id,
                            LecturerId=lec.Id,
                            FavoritePoint=3,
                            FeedbackPoint=3,
                            MaxCourseSubject=3,
                            IsEnable=1,
                            Status=1
                        };
                        await _res.InsertAsync(subjectOfLecturer);
                        await _res.SaveAsync();
                    }
                }


                return new ApiResponse
                {
                    Success = true,
                    Message = "Create SubjectOfLecturer In Semester Success"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Create SubjectOfLecturer In Semester Fail",
                    Data = ex.Message
                };
            }
        }

        public async Task<ApiResponse> DeleteSubjectOfLecturerInSemester(string semesterID)
        {
            try
            {
                var listSubjectOfLecturer = await _res.GetAllByIQueryable()
                    .Where(x => x.SemesterId == semesterID && x.Status == (int)SubjectOfLecturerStatus.Active)
                    .ToListAsync();
                if (listSubjectOfLecturer.Count == 0)
                {
                    return new ApiResponse()
                    {
                        Success = true,
                        Message = "Delete SubjectOfLecturer In Semester Success",
                        Data = "List already is empty"
                    };
                }
                else
                {
                    foreach (var SlotType in listSubjectOfLecturer)
                    {
                        await _res.DeleteAsync(SlotType.Id);
                    }
                    return new ApiResponse()
                    {
                        Success = true,
                        Message = "Delete SubjectOfLecturer In Semester Success"
                    };
                }

            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Success = false,
                    Message = "Delete SubjectOfLecturer In Semester Fail",
                    Data = ex.Message
                };
            }
        }
    }
}
