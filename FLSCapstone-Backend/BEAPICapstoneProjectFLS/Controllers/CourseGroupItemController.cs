using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.CourseGroupItemRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseGroupItemController : ControllerBase
    {
        private readonly ICourseGroupItemService _ICourseGroupItemService;

        public CourseGroupItemController(ICourseGroupItemService CourseGroupItemService)
        {
            _ICourseGroupItemService = CourseGroupItemService;
        }

        [HttpGet("{id}", Name = "GetCourseGroupItemById")]
        public async Task<IActionResult> GetCourseGroupItemById(string id)
        {
            var CourseGroupItemModel = await _ICourseGroupItemService.GetCourseGroupItemById(id);
            if (CourseGroupItemModel == null)
                return NotFound();
            return Ok(CourseGroupItemModel);
        }

        [HttpGet]
        public IActionResult GetAllCourseGroupItem([FromQuery] CourseGroupItemViewModel flitter, CourseGroupItemSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listCourseGroupItemModel = _ICourseGroupItemService.GetAllCourseGroupItem(flitter, pageIndex, pageSize, sortBy, order);
            return Ok(listCourseGroupItemModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourseGroupItem([FromBody] CreateCourseGroupItemRequest request)
        {
            var CourseGroupItemVM = await _ICourseGroupItemService.CreateCourseGroupItem(request);
            if (CourseGroupItemVM == null)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute(nameof(GetCourseGroupItemById), new { id = CourseGroupItemVM.Id }, CourseGroupItemVM);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourseGroupItem(string id, [FromBody] UpdateCourseGroupItemRequest request)
        {
            var CourseGroupItemVM = await _ICourseGroupItemService.UpdateCourseGroupItem(id, request);
            if (CourseGroupItemVM == null)
                return BadRequest();
            return Ok(CourseGroupItemVM);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseGroupItem(string id)
        {
            var rs = await _ICourseGroupItemService.DeleteCourseGroupItem(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }

        [HttpDelete("DeleteCourseGroupItemInSemester/{semesterID}")]
        public async Task<IActionResult> DeleteCourseGroupItemInSemester(string semesterID)
        {
            var rs = await _ICourseGroupItemService.DeleteCourseGroupItemInSemester(semesterID);
            if (rs == false)
                return NotFound();
            return Ok();
        }
    }
}
