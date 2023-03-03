using AutoMapper;
using AutoMapper.QueryableExtensions;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.IRepositories;
using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.RandomKey;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.EntityFrameworkCore;
using PagedList;
using Reso.Core.Utilities;
using System.Linq;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IGenericRepository<Department> _res;
        private readonly IMapper _mapper;

        public DepartmentService(IGenericRepository<Department> repository, IMapper mapper)
        {
            _res = repository;
            _mapper = mapper;
        }

        public async Task<DepartmentViewModel> CreateDepartment(CreateDepartmentRequest request)
        {
            try
            {
                var de = _mapper.Map<Department>(request);
                await _res.InsertAsync(de);
                await _res.SaveAsync();

                var d = await GetDepartmentById(de.Id);
                return d;
            }
            catch
            {
                return null;
            }         
        }

        public async Task<bool> DeleteDepartment(string id)
        {
            var department = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)DepartmentStatus.Active)).FirstOrDefault();
            if (department == null)
            {
                return false;
            }
            department.Status = 0;
            await _res.UpdateAsync(department);
            await _res.SaveAsync();
            return true;
        }

        public IPagedList<DepartmentViewModel> GetAllDepartment(DepartmentViewModel flitter, int pageIndex, int pageSize, DepartmentSortBy sortBy, OrderBy order)
        {
            var listDepartment = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

   
            var listDepartmentViewModel = (listDepartment.ProjectTo<DepartmentViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "DepartmentGroupName":
                    if (order.ToString() == "Asc")
                    {
                        listDepartmentViewModel = (IQueryable<DepartmentViewModel>)listDepartmentViewModel.OrderBy(x => x.DepartmentName);
                    }
                    else
                    {
                        listDepartmentViewModel = (IQueryable<DepartmentViewModel>)listDepartmentViewModel.OrderByDescending(x => x.DepartmentName);
                    }
                    break;
                case "Id":
                    if (order.ToString() == "Asc")
                    {
                        listDepartmentViewModel = (IQueryable<DepartmentViewModel>)listDepartmentViewModel.OrderBy(x => x.Id);
                    }
                    else
                    {
                        listDepartmentViewModel = (IQueryable<DepartmentViewModel>)listDepartmentViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<DepartmentViewModel>(listDepartmentViewModel, pageIndex, pageSize);
        }

        public async Task<DepartmentViewModel> GetDepartmentById(string id)
        {
            var de = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)DepartmentStatus.Active)
                .Include(x => x.DepartmentGroup)
                .FirstOrDefaultAsync();
            if (de == null)
                return null;
            var departmentVM = _mapper.Map<DepartmentViewModel>(de);
            return departmentVM;
        }

        public async Task<DepartmentViewModel> UpdateDepartment(string id, UpdateDepartmentRequest request)
        {
            try
            {
                var listDepartment = await _res.FindByAsync(x => x.Id == id && x.Status == (int)DepartmentStatus.Active);
                if (listDepartment == null)
                {
                    return null;
                }
                var department = listDepartment.FirstOrDefault();
                if (department == null)
                {
                    return null;
                }
                department = _mapper.Map(request, department);
                await _res.UpdateAsync(department);
                await _res.SaveAsync();

                var d = await GetDepartmentById(department.Id);
                return d;
            }
            catch
            {
                return null;
            }
            
        }
    }
}
