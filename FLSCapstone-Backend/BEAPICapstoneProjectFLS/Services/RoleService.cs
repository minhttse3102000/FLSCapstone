using AutoMapper;
using AutoMapper.QueryableExtensions;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.IRepositories;
using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.RandomKey;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.RoleRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.EntityFrameworkCore;
using PagedList;
using Reso.Core.Utilities;
using System.Linq;
using System.Threading.Tasks;
namespace BEAPICapstoneProjectFLS.Services
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _res;
        private readonly IMapper _mapper;

        public RoleService(IGenericRepository<Role> repository, IMapper mapper)
        {
            _res = repository;
            _mapper = mapper;
        }

        public async Task<RoleViewModel> CreateRole(CreateRoleRequest request)
        {
            try
            {
                var role = _mapper.Map<Role>(request);
                role.Id = RandomPKKey.NewRamDomPKKey();
                await _res.InsertAsync(role);
                await _res.SaveAsync();

                var roleVM = await GetRoleById(role.Id);
                return roleVM;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteRole(string id)
        {
            var role = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)RoleStatus.Active)).FirstOrDefault();
            if (role == null)
            {
                return false;
            }
            role.Status = 0;
            await _res.UpdateAsync(role);
            await _res.SaveAsync();
            return true;
        }

        public IPagedList<RoleViewModel> GetAllRole(RoleViewModel flitter, int pageIndex, int pageSize, RoleSortBy sortBy, OrderBy order)
        {
            var listRole = _res.FindBy(x => x.Status == (int)FLSStatus.Active);


            var listRoleViewModel = (listRole.ProjectTo<RoleViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "RoleName":
                    if (order.ToString() == "Asc")
                    {
                        listRoleViewModel = (IQueryable<RoleViewModel>)listRoleViewModel.OrderBy(x => x.RoleName);
                    }
                    else
                    {
                        listRoleViewModel = (IQueryable<RoleViewModel>)listRoleViewModel.OrderByDescending(x => x.RoleName);
                    }
                    break;
                case "Id":
                    if (order.ToString() == "Asc")
                    {
                        listRoleViewModel = (IQueryable<RoleViewModel>)listRoleViewModel.OrderBy(x => x.Id);
                    }
                    else
                    {
                        listRoleViewModel = (IQueryable<RoleViewModel>)listRoleViewModel.OrderByDescending(x => x.Id);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<RoleViewModel>(listRoleViewModel, pageIndex, pageSize);
        }

        public async Task<RoleViewModel> GetRoleById(string id)
        {
            var role = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)RoleStatus.Active)
                .FirstOrDefaultAsync();
            if (role == null)
                return null;
            var roleVM = _mapper.Map<RoleViewModel>(role);
            return roleVM;
        }

        public async Task<RoleViewModel> UpdateRole(string id, UpdateRoleRequest request)
        {
            try
            {
                var listRole = await _res.FindByAsync(x => x.Id == id && x.Status == (int)RoleStatus.Active);
                if (listRole == null)
                {
                    return null;
                }
                var role = listRole.FirstOrDefault();
                if (role == null)
                {
                    return null;
                }
                role = _mapper.Map(request, role);
                await _res.UpdateAsync(role);
                await _res.SaveAsync();

                var roleVM = await GetRoleById(role.Id);
                return roleVM;
            }
            catch
            {
                return null;
            }

        }
    }
}
