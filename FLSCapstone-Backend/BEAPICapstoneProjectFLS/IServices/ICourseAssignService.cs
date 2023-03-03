using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.CourseAssignRequest;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace BEAPICapstoneProjectFLS.IServices
{
    public interface ICourseAssignService
    {
        Task<CourseAssignViewModel> GetCourseAssignById(string id);
        Task<IEnumerable<CourseAssignViewModel>> GetCourseAssignByGroup(string GroupID);
        IPagedList<CourseAssignViewModel> GetAllCourseAssign(CourseAssignViewModel flitter, int pageIndex,
           int pageSize, CourseAssignSortBy sortBy, OrderBy order);
        Task<IEnumerable<UserViewModel>> GetUserAssignInDepartment(string subjectID, string semesterID);
        Task<IEnumerable<UserViewModel>> GetUserAssignOutDepartment(string subjectID, string semesterID);
        Task<CourseAssignViewModel> CreateCourseAssign(CreateCourseAssignRequest request);
        Task<CourseAssignViewModel> UpdateCourseAssign(string id, UpdateCourseAssignRequest request);
        Task<bool> DeleteCourseAssign(string id);

        Task<bool> CreateListCourseAssign(string ScheduleID, List<CreateCourseAssignRequest> requests);

        Task<bool> DeleteListCourseAssignInSemester(string ScheduleID);
        Task<bool> DeleteAssignedCourses(string ScheduleID);
    }
}
