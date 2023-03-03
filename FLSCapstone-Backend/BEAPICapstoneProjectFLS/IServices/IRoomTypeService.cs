using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.Requests.RoomTypeRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Threading.Tasks;


namespace BEAPICapstoneProjectFLS.IServices
{
    public interface IRoomTypeService
    {
        Task<RoomTypeViewModel> GetRoomTypeById(string id);
        IPagedList<RoomTypeViewModel> GetAllRoomType(RoomTypeViewModel flitter, int pageIndex,
           int pageSize, RoomTypeSortBy sortBy, OrderBy order);
        Task<RoomTypeViewModel> CreateRoomType(CreateRoomTypeRequest request);
        Task<RoomTypeViewModel> UpdateRoomType(string id, UpdateRoomTypeRequest request);
        Task<bool> DeleteRoomType(string id);
    }
}
