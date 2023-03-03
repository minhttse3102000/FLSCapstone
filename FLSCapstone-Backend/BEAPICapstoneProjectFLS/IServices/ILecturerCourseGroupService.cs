using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.Requests.LecturerCourseGroupRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.IServices
{
    public interface ILecturerCourseGroupService
    {
        Task<LecturerCourseGroupViewModel> GetLecturerCourseGroupById(string id);
        IPagedList<LecturerCourseGroupViewModel> GetAllLecturerCourseGroup(LecturerCourseGroupViewModel flitter, int pageIndex,
           int pageSize, LecturerCourseGroupSortBy sortBy, OrderBy order);
        Task<LecturerCourseGroupViewModel> CreateLecturerCourseGroup(CreateLecturerCourseGroupRequest request);
        Task<LecturerCourseGroupViewModel> UpdateLecturerCourseGroup(string id, UpdateLecturerCourseGroupRequest request);
        Task<bool> DeleteLecturerCourseGroup(string id);
        Task<ApiResponse> CreateLecturerCourseGroupInSemester(string semesterID);
        Task<ApiResponse> DeleteLecturerCourseGroupInSemester(string semesterID);
    }
}
