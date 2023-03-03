using AutoMapper;
using AutoMapper.QueryableExtensions;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Enum;
using BEAPICapstoneProjectFLS.IRepositories;
using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.RandomKey;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.UserAndRoleRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.EntityFrameworkCore;
using PagedList;
using Reso.Core.Utilities;
using System.Linq;
using System.Threading.Tasks;
namespace BEAPICapstoneProjectFLS.Services
{
    public class UserAndRoleService : IUserAndRoleService
    {
        private readonly IGenericRepository<UserAndRole> _res;
        private readonly IMapper _mapper;

        public UserAndRoleService(IGenericRepository<UserAndRole> repository, IMapper mapper)
        {
            _res = repository;
            _mapper = mapper;
        }

        public async Task<UserAndRoleViewModel> CreateUserAndRole(CreateUserAndRoleRequest request)
        {
            try
            {
                var uar = _mapper.Map<UserAndRole>(request);
                uar.Id = RandomPKKey.NewRamDomPKKey();
                await _res.InsertAsync(uar);
                await _res.SaveAsync();

                var uarVM = await GetUserAndRoleById(uar.Id);
                return uarVM;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteUserAndRole(string id)
        {
            var userAndRole = (await _res.FindByAsync(x => x.Id == id && x.Status == (int)UserAndRoleStatus.Active)).FirstOrDefault();
            if (userAndRole == null)
            {
                return false;
            }
            userAndRole.Status = 0;
            await _res.UpdateAsync(userAndRole);
            await _res.SaveAsync();
            return true;
        }

        public IPagedList<UserAndRoleViewModel> GetAllUserAndRole(UserAndRoleViewModel flitter, int pageIndex, int pageSize, UserAndRoleSortBy sortBy, OrderBy order)
        {
            var listUserAndRole = _res.FindBy(x => x.Status == (int)FLSStatus.Active);


            var listUserAndRoleViewModel = (listUserAndRole.ProjectTo<UserAndRoleViewModel>
                (_mapper.ConfigurationProvider)).DynamicFilter(flitter);
            switch (sortBy.ToString())
            {
                case "UserId":
                    if (order.ToString() == "Asc")
                    {
                        listUserAndRoleViewModel = (IQueryable<UserAndRoleViewModel>)listUserAndRoleViewModel.OrderBy(x => x.UserId);
                    }
                    else
                    {
                        listUserAndRoleViewModel = (IQueryable<UserAndRoleViewModel>)listUserAndRoleViewModel.OrderByDescending(x => x.UserId);
                    }
                    break;
                case "RoleId":
                    if (order.ToString() == "Asc")
                    {
                        listUserAndRoleViewModel = (IQueryable<UserAndRoleViewModel>)listUserAndRoleViewModel.OrderBy(x => x.RoleId);
                    }
                    else
                    {
                        listUserAndRoleViewModel = (IQueryable<UserAndRoleViewModel>)listUserAndRoleViewModel.OrderByDescending(x => x.RoleId);
                    }
                    break;
            }
            return PagedListExtensions.ToPagedList<UserAndRoleViewModel>(listUserAndRoleViewModel, pageIndex, pageSize);
        }

        public async Task<UserAndRoleViewModel> GetUserAndRoleById(string id)
        {
            var uar = await _res.GetAllByIQueryable()
                .Where(x => x.Id == id && x.Status == (int)UserAndRoleStatus.Active)
                .Include(x => x.Role)
                .Include(x => x.User)
                .FirstOrDefaultAsync();
            if (uar == null)
                return null;
            var userAndRoleVM = _mapper.Map<UserAndRoleViewModel>(uar);
            return userAndRoleVM;
        }

        public async Task<UserAndRoleViewModel> UpdateUserAndRole(string id, UpdateUserAndRoleRequest request)
        {
            try
            {
                var listUserAndRole = await _res.FindByAsync(x => x.Id == id && x.Status == (int)UserAndRoleStatus.Active);
                if (listUserAndRole == null)
                {
                    return null;
                }
                var userAndRole = listUserAndRole.FirstOrDefault();
                if (userAndRole == null)
                {
                    return null;
                }
                userAndRole = _mapper.Map(request, userAndRole);
                await _res.UpdateAsync(userAndRole);
                await _res.SaveAsync();

                var uarVM = await GetUserAndRoleById(userAndRole.Id);
                return uarVM;
            }
            catch
            {
                return null;
            }

        }
    }
}
