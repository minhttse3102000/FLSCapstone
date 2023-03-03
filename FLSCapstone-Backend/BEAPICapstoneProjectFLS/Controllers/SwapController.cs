using BEAPICapstoneProjectFLS.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SwapController : ControllerBase
    {
        private readonly ISwapService _ISwapService;
        private readonly ICourseAssignService _ICourseAssignService;

        public SwapController(ISwapService SwapService, ICourseAssignService CourseAssignService)
        {
            _ISwapService = SwapService;
            _ICourseAssignService = CourseAssignService;
        }

        [HttpGet("BlankSlot/{courseAssignID}", Name = "GetBlankSlot")]
        public async Task<IActionResult> GetBlankSlot(string courseAssignID)
        {
            var courseAssignVM = await _ICourseAssignService.GetCourseAssignById(courseAssignID);
            if (courseAssignVM == null)
            {
                return NotFound();
            }
            else
            {
                var slotTypeVM = await _ISwapService.GetBlankSlot(courseAssignID);
                if (slotTypeVM == null)
                    return NotFound();
                return Ok(slotTypeVM);
            }
        }


        [HttpGet("CourseAssignToSwap/{courseAssignID}", Name = "GetCourseAssignToSwap")]
        public async Task<IActionResult> GetCourseAssignToSwap(string courseAssignID)
        {
            var courseAssignVM= await _ICourseAssignService.GetCourseAssignById(courseAssignID);
            if(courseAssignVM == null)
            {
                return NotFound();
            }
            else
            {
                var courseAssignVMSwap = await _ISwapService.GetCourseAssignToSwap(courseAssignID);
                if (courseAssignVMSwap == null)
                    return NotFound();
                return Ok(courseAssignVMSwap);
            }


        }



    }
}
