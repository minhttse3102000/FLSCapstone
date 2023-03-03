using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.Requests.DepartmentRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Threading.Tasks;
namespace BEAPICapstoneProjectFLS.IServices
{
    public interface IDepartmentService
    {
        Task<DepartmentViewModel> GetDepartmentById(string id);
        IPagedList<DepartmentViewModel> GetAllDepartment(DepartmentViewModel flitter, int pageIndex,
           int pageSize, DepartmentSortBy sortBy, OrderBy order);
        Task<DepartmentViewModel> CreateDepartment(CreateDepartmentRequest request);
        Task<DepartmentViewModel> UpdateDepartment(string id, UpdateDepartmentRequest request);
        Task<bool> DeleteDepartment(string id);
    }
}
