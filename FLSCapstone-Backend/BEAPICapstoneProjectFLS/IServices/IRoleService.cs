using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.RoleRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Threading.Tasks;
namespace BEAPICapstoneProjectFLS.IServices
{
    public interface IRoleService
    {
        Task<RoleViewModel> GetRoleById(string id);
        IPagedList<RoleViewModel> GetAllRole(RoleViewModel flitter, int pageIndex,
           int pageSize, RoleSortBy sortBy, OrderBy order);
        Task<RoleViewModel> CreateRole(CreateRoleRequest request);
        Task<RoleViewModel> UpdateRole(string id, UpdateRoleRequest request);
        Task<bool> DeleteRole(string id);
    }
}
