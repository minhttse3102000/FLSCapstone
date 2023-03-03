using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.Requests.SlotTypeRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.IServices
{
    public interface ISlotTypeService
    {
        Task<SlotTypeViewModel> GetSlotTypeById(string id);
        IPagedList<SlotTypeViewModel> GetAllSlotType(SlotTypeViewModel flitter, int pageIndex,
           int pageSize, SlotTypeSortBy sortBy, OrderBy order);
        Task<SlotTypeViewModel> CreateSlotType(CreateSlotTypeRequest request);
        Task<SlotTypeViewModel> UpdateSlotType(string id, UpdateSlotTypeRequest request);
        Task<bool> DeleteSlotType(string id);

        Task<ApiResponse> CreateListSlotType(string semesterID, List<CreateSlotTypeInSemesterRequest> requests);

        Task<bool> DeleteListSlotTypeInSemester(string SemesterID);
    }
}
