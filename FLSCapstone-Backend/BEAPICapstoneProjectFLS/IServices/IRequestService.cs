using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.Requests.Request;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Threading.Tasks;
namespace BEAPICapstoneProjectFLS.IServices
{
    public interface IRequestService
    {
        Task<RequestViewModel> GetRequestById(string id);
        IPagedList<RequestViewModel> GetAllRequest(RequestViewModel flitter, int pageIndex,
           int pageSize, RequestSortBy sortBy, OrderBy order);
        Task<RequestViewModel> CreateRequest(CreateRequest request);
        Task<RequestViewModel> UpdateRequest(string id, UpdateRequest request);
        Task<bool> DeleteRequest(string id);
        Task<ApiResponse> DeleteRequestInSemester(string semesterID);
    }
}
