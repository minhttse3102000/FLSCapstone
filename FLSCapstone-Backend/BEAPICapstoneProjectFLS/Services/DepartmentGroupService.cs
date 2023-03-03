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

namespace BEAPICapstoneProjectFLS.Services
{
    public class DepartmentGroupService : IDepartmentGroupService
    {
        private readonly IGenericRepository<DepartmentGroup> _res;
        private readonly IMapper _mapper;

        public DepartmentGroupService(IGenericRepository<DepartmentGroup> repository, IMapper mapper)
        {
            _res = repository;
            _mapper = mapper;
        }

        public async Task<DepartmentGroupViewModel> CreateDepartmentGroup(CreateDepartmentGroupRequest request)
        {
            try
            {
                var dg = _mapper.Map<DepartmentGroup>(request);
                dg.Id = RandomPKKey.NewRamDomPKKey();
                await _res.InsertAsync(dg);
                await _res.SaveAsync();

                var dgVM = await GetDepartmentGroupById(dg.Id);
                return dgVM;
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> DeleteDepartmentGroup(string id)
        {
            var departmentGroup = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)DepartmentGroupStatus.Active)).FirstOrDefault();
            if (departmentGroup == null)
            {
                return false;
            }
            departmentGroup.Status = 0;
            await _res.UpdateAsync(departmentGroup);
            await _res.SaveAsync();
            return true;
        }

        public IPagedList<DepartmentGroupViewModel> GetAllDepartmentGroup(DepartmentGroupViewModel flitter, int pageIndex, int pageSize, DepartmentGroupSortBy sortBy, OrderBy order)
        {
            var listDepartmentGroup = _res.FindBy(x => x.Status == (int)FLSStatus.Active);

            var listDepartmentGroupViewModel = (listDepartmentGroup.ProjectTo<DepartmentGroupViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "DepartmentGroupName":
                    if (order.ToString() == "Asc")
                    {
                        listDepartmentGroupViewModel = (IQueryable<DepartmentGroupViewModel>)listDepartmentGroupViewModel.OrderBy(x => x.DepartmentGroupName);
                    }
                    else
                    {
                        listDepartmentGroupViewModel = (IQueryable<DepartmentGroupViewModel>)listDepartmentGroupViewModel.OrderByDescending(x => x.DepartmentGroupName);
                    }
                    break;
                case "Id":
                    if (order.ToString() == "Asc")
                    {
                        listDepartmentGroupViewModel = (IQueryable<DepartmentGroupViewModel>)listDepartmentGroupViewModel.OrderBy(x => x.Id);
                    }
                    else
                    {
                        listDepartmentGroupViewModel = (IQueryable<DepartmentGroupViewModel>)listDepartmentGroupViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<DepartmentGroupViewModel>(listDepartmentGroupViewModel, pageIndex, pageSize);
        }


        public async Task<DepartmentGroupViewModel> GetDepartmentGroupById(string id)
        {
          
            var dg = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)DepartmentGroupStatus.Active)
                .Include(x => x.Departments)
                .FirstOrDefaultAsync();
            if (dg == null)
                return null;
            var departmentGroupVM = _mapper.Map<DepartmentGroupViewModel>(dg);
            return departmentGroupVM;
        }

        public async Task<DepartmentGroupViewModel> UpdateDepartmentGroup(string id, UpdateDepartmentGroupRequest request)
        {
            try
            {
                var listDepartmentGroup = await _res.FindByAsync(x => x.Id == id && x.Status == (int)DepartmentGroupStatus.Active);
                if (listDepartmentGroup == null)
                {
                    return null;
                }
                var departmentGroup = listDepartmentGroup.FirstOrDefault();
                if (departmentGroup == null)
                {
                    return null;
                }
                departmentGroup = _mapper.Map(request, departmentGroup);
                await _res.UpdateAsync(departmentGroup);
                await _res.SaveAsync();

                var dgVM = await GetDepartmentGroupById(departmentGroup.Id);
                return dgVM;
            }
            catch
            {
                return null;    
            }           
        }
    }
}
