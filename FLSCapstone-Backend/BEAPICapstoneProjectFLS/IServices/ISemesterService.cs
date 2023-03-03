using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.Requests.SemesterRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Threading.Tasks;
namespace BEAPICapstoneProjectFLS.IServices
{
    public interface ISemesterService
    {
        Task<SemesterViewModel> GetSemesterById(string id);
        IPagedList<SemesterViewModel> GetAllSemester(SemesterViewModel flitter, int pageIndex,
           int pageSize, SemesterSortBy sortBy, OrderBy order);
        Task<SemesterViewModel> CreateSemester(CreateSemesterRequest request);
        Task<SemesterViewModel> UpdateSemester(string id, UpdateSemesterRequest request);
        Task<bool> DeleteSemester(string id);
    }
}
