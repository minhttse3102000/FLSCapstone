using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.UserRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Threading.Tasks;
namespace BEAPICapstoneProjectFLS.IServices
{
    public interface IUserService
    {
        Task<UserViewModel> GetUserById(string id);
        Task<UserViewModel> GetUserByEmail(string email);
        IPagedList<UserViewModel> GetAllUser(UserViewModel flitter, int pageIndex,
           int pageSize, UserSortBy sortBy, OrderBy order);
        Task<UserViewModel> CreateUser(CreateUserRequest request);
        Task<UserViewModel> UpdateUser(string id, UpdateUserRequest request);
        Task<bool> DeleteUser(string id);
    }
}
