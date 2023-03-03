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
using BEAPICapstoneProjectFLS.Requests.SemesterRequest;

namespace BEAPICapstoneProjectFLS.Services
{
    public class SemesterService: ISemesterService
    {
        private readonly IGenericRepository<Semester> _res;
        private readonly IMapper _mapper;

        public SemesterService(IGenericRepository<Semester> repository, IMapper mapper)
        {
            _res = repository;
            _mapper = mapper;
        }

        public async Task<SemesterViewModel> CreateSemester(CreateSemesterRequest request)
        {
            try
            {
                var se = _mapper.Map<Semester>(request);
                se.Id = RandomPKKey.NewRamDomPKKey();
                await _res.InsertAsync(se);
                await _res.SaveAsync();

                var seVM = await GetSemesterById(se.Id);
                return seVM;
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> DeleteSemester(string id)
        {
            var semester = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)SemesterStatus.Active)).FirstOrDefault();
            if (semester == null)
            {
                return false;
            }
            semester.Status = 0;
            await _res.UpdateAsync(semester);
            await _res.SaveAsync();
            return true;
        }

        public IPagedList<SemesterViewModel> GetAllSemester(SemesterViewModel flitter, int pageIndex, int pageSize, SemesterSortBy sortBy, OrderBy order)
        {
            var listSemester = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

            var listSemesterViewModel = (listSemester.ProjectTo<SemesterViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "Term":
                    if (order.ToString() == "Asc")
                    {
                        listSemesterViewModel = (IQueryable<SemesterViewModel>)listSemesterViewModel.OrderBy(x => x.Term);
                    }
                    else
                    {
                        listSemesterViewModel = (IQueryable<SemesterViewModel>)listSemesterViewModel.OrderByDescending(x => x.Term);
                    }
                    break;
                case "Id":
                    if (order.ToString() == "Asc")
                    {
                        listSemesterViewModel = (IQueryable<SemesterViewModel>)listSemesterViewModel.OrderBy(x => x.Id);
                    }
                    else
                    {
                        listSemesterViewModel = (IQueryable<SemesterViewModel>)listSemesterViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
                case "DateEnd":
                    if (order.ToString() == "Asc")
                    {
                        listSemesterViewModel = (IQueryable<SemesterViewModel>)listSemesterViewModel.OrderBy(x => x.DateEnd);
                    }
                    else
                    {
                        listSemesterViewModel = (IQueryable<SemesterViewModel>)listSemesterViewModel.OrderByDescending(x => x.DateEnd);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<SemesterViewModel>(listSemesterViewModel, pageIndex, pageSize);
        }


        public async Task<SemesterViewModel> GetSemesterById(string id)
        {
            //var departmentGetByID = _context.Departments.SingleOrDefault(ty => ty.DepartmentID == id);

            var se = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)SemesterStatus.Active)
                .FirstOrDefaultAsync();
            //var dg = await _res.GetByIDAsync(id);
            if (se == null)
                return null;
            var SemesterVM = _mapper.Map<SemesterViewModel>(se);
            var test = _mapper.Map<Semester>(SemesterVM);
            return SemesterVM;
        }

        public async Task<SemesterViewModel> UpdateSemester(string id, UpdateSemesterRequest request)
        {
            try
            {
                var listSemester = await _res.FindByAsync(x => x.Id == id && x.Status == (int)SemesterStatus.Active);
                if (listSemester == null)
                {
                    return null;
                }
                var semester = listSemester.FirstOrDefault();
                if (semester == null)
                {
                    return null;
                }
                semester = _mapper.Map(request, semester);
                await _res.UpdateAsync(semester);
                await _res.SaveAsync();

                var seVM = await GetSemesterById(semester.Id);
                return seVM;
            }
            catch
            {
                return null;
            }
        }

    }
}
