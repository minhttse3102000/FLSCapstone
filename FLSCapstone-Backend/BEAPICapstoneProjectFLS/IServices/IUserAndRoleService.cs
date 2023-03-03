using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.UserAndRoleRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Threading.Tasks;
namespace BEAPICapstoneProjectFLS.IServices
{
    public interface IUserAndRoleService
    {
        Task<UserAndRoleViewModel> GetUserAndRoleById(string id);
        IPagedList<UserAndRoleViewModel> GetAllUserAndRole(UserAndRoleViewModel flitter, int pageIndex,
           int pageSize, UserAndRoleSortBy sortBy, OrderBy order);
        Task<UserAndRoleViewModel> CreateUserAndRole(CreateUserAndRoleRequest request);
        Task<UserAndRoleViewModel> UpdateUserAndRole(string id, UpdateUserAndRoleRequest request);
        Task<bool> DeleteUserAndRole(string id);
    }
}
