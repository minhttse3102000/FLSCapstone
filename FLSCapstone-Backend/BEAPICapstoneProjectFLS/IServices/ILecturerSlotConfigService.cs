using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.Requests.LecturerSlotConfigRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using PagedList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.IServices
{
    public interface ILecturerSlotConfigService
    {
        Task<LecturerSlotConfigViewModel> GetLecturerSlotConfigById(string id);
        IPagedList<LecturerSlotConfigViewModel> GetAllLecturerSlotConfig(LecturerSlotConfigViewModel flitter, int pageIndex,
           int pageSize, LecturerSlotConfigSortBy sortBy, OrderBy order);
        Task<LecturerSlotConfigViewModel> CreateLecturerSlotConfig(CreateLecturerSlotConfigRequest request);
        Task<LecturerSlotConfigViewModel> UpdateLecturerSlotConfig(string id, UpdateLecturerSlotConfigRequest request);
        Task<bool> DeleteLecturerSlotConfig(string id);
        Task<ApiResponse> CreateSlotTypesAndLecturerSlotConfigsInSemester(string semesterID);
        Task<ApiResponse> DeleteLecturerSlotConfigInSemester(string semesterID);
    }
}
