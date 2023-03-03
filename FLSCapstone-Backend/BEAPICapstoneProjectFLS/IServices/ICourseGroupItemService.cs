using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.CourseGroupItemRequest;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.IServices
{
    public interface ICourseGroupItemService
    {
        Task<CourseGroupItemViewModel> GetCourseGroupItemById(string id);
        IPagedList<CourseGroupItemViewModel> GetAllCourseGroupItem(CourseGroupItemViewModel flitter, int pageIndex,
           int pageSize, CourseGroupItemSortBy sortBy, OrderBy order);
        Task<CourseGroupItemViewModel> CreateCourseGroupItem(CreateCourseGroupItemRequest request);
        Task<CourseGroupItemViewModel> UpdateCourseGroupItem(string id, UpdateCourseGroupItemRequest request);
        Task<bool> DeleteCourseGroupItem(string id);
        Task<bool> DeleteCourseGroupItemInSemester(string semesterID);
    }
}
