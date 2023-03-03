using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.Requests.SubjectOfLecturerRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.IServices
{
    public interface ISubjectOfLecturerService
    {
        Task<SubjectOfLecturerViewModel> GetSubjectOfLecturerById(string id);
        IPagedList<SubjectOfLecturerViewModel> GetAllSubjectOfLecturer(SubjectOfLecturerViewModel flitter, int pageIndex,
           int pageSize, SubjectOfLecturerSortBy sortBy, OrderBy order);
        Task<SubjectOfLecturerViewModel> CreateSubjectOfLecturer(CreateSubjectOfLecturerRequest request);
        Task<SubjectOfLecturerViewModel> UpdateSubjectOfLecturer(string id, UpdateSubjectOfLecturerRequest request);
        Task<bool> DeleteSubjectOfLecturer(string id);
        Task<ApiResponse> CreateSubjectOfLecturerInSemester(string semesterID);
        Task<ApiResponse> DeleteSubjectOfLecturerInSemester(string semesterID);
    }
}
