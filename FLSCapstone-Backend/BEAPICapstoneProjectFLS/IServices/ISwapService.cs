using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.IServices
{
    public interface ISwapService
    {
        Task<IEnumerable<SlotTypeViewModel>> GetBlankSlot(string courseAssignID);
        Task<IEnumerable<CourseAssignViewModel>> GetCourseAssignToSwap(string courseAssignID);
    }
}
