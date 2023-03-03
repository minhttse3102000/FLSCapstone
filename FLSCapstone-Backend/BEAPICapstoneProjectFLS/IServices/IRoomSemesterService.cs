using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.Requests.RoomSemesterRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.IServices
{
    public interface IRoomSemesterService
    {
        Task<RoomSemesterViewModel> GetRoomSemesterById(string id);
        IPagedList<RoomSemesterViewModel> GetAllRoomSemester(RoomSemesterViewModel flitter, int pageIndex,
           int pageSize, RoomSemesterSortBy sortBy, OrderBy order);
        Task<RoomSemesterViewModel> CreateRoomSemester(CreateRoomSemesterRequest request);
        Task<RoomSemesterViewModel> UpdateRoomSemester(string id, UpdateRoomSemesterRequest request);
        Task<bool> DeleteRoomSemester(string id);
    }
}
