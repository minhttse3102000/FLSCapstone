using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.Requests.DepartmentRequest;
using BEAPICapstoneProjectFLS.Requests.ScheduleRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Threading.Tasks;
namespace BEAPICapstoneProjectFLS.IServices
{
    public interface IScheduleService
    {
        Task<ScheduleViewModel> GetScheduleById(string id);
        IPagedList<ScheduleViewModel> GetAllSchedule(ScheduleViewModel flitter, int pageIndex,
           int pageSize, ScheduleSortBy sortBy, OrderBy order);
        Task<ScheduleViewModel> CreateSchedule(CreateScheduleRequest request);
        Task<ScheduleViewModel> UpdateSchedule(string id, UpdateScheduleRequest request);
        Task<bool> DeleteSchedule(string id);
    }
}
