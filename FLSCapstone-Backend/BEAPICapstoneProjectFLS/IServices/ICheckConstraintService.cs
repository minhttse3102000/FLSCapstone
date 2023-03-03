using BEAPICapstoneProjectFLS.Services;
using BEAPICapstoneProjectFLS.ViewModel;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.IServices
{
    public interface ICheckConstraintService 
    {
        Task<ApiResponse> CheckSemesterPublic(string semesterID);
        Task<ApiResponse> CheckAllDepartmentManagerConfirm(string semesterID);
        Task<ApiResponse> CheckCourseOflecrurer(string lecturerID, string semesterID);
        Task<ApiResponse> SetAllDepartmentManagerConfirm(string semesterID);
    }
}
