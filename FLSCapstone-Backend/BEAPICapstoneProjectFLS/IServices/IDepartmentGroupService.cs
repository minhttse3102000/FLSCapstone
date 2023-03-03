using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.IServices
{
    public interface IDepartmentGroupService
    {
        Task<DepartmentGroupViewModel> GetDepartmentGroupById(string id);
        IPagedList<DepartmentGroupViewModel> GetAllDepartmentGroup(DepartmentGroupViewModel flitter, int pageIndex,
           int pageSize, DepartmentGroupSortBy sortBy, OrderBy order);
        Task<DepartmentGroupViewModel> CreateDepartmentGroup(CreateDepartmentGroupRequest request);
        Task<DepartmentGroupViewModel> UpdateDepartmentGroup(string id, UpdateDepartmentGroupRequest request);
        Task<bool> DeleteDepartmentGroup(string id);
    }
}
