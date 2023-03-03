using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.LecturerCourseGroupRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerCourseGroupController : ControllerBase
    {
        private readonly ILecturerCourseGroupService _ILecturerCourseGroupService;
        private readonly ISemesterService _ISemesterService;

        public LecturerCourseGroupController(ILecturerCourseGroupService LecturerCourseGroupService, ISemesterService SemesterService)
        {
            _ILecturerCourseGroupService = LecturerCourseGroupService;
            _ISemesterService = SemesterService;
        }

        [HttpGet("{id}", Name = "GetLecturerCourseGroupById")]
        public async Task<IActionResult> GetLecturerCourseGroupById(string id)
        {
            var LecturerCourseGroupModel = await _ILecturerCourseGroupService.GetLecturerCourseGroupById(id);
            if (LecturerCourseGroupModel == null)
                return NotFound();
            return Ok(LecturerCourseGroupModel);
        }

        [HttpGet]
        public IActionResult GetAllLecturerCourseGroup([FromQuery] LecturerCourseGroupViewModel flitter, LecturerCourseGroupSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listLecturerCourseGroupModel = _ILecturerCourseGroupService.GetAllLecturerCourseGroup(flitter, pageIndex, pageSize, sortBy, order);
            return Ok(listLecturerCourseGroupModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLecturerCourseGroup([FromBody] CreateLecturerCourseGroupRequest request)
        {
            var LecturerCourseGroupVM = await _ILecturerCourseGroupService.CreateLecturerCourseGroup(request);
            if (LecturerCourseGroupVM == null)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute(nameof(GetLecturerCourseGroupById), new { id = LecturerCourseGroupVM.Id }, LecturerCourseGroupVM);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLecturerCourseGroup(string id, [FromBody] UpdateLecturerCourseGroupRequest request)
        {
            var LecturerCourseGroupVM = await _ILecturerCourseGroupService.UpdateLecturerCourseGroup(id, request);
            if (LecturerCourseGroupVM == null)
                return BadRequest();
            return Ok(LecturerCourseGroupVM);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLecturerCourseGroup(string id)
        {
            var rs = await _ILecturerCourseGroupService.DeleteLecturerCourseGroup(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }

        [HttpPost("CreateLcgNewSemester/{semesterID}", Name = "CreateLecturerCourseGroupInSemester")]
        public async Task<IActionResult> CreateLecturerCourseGroupInSemester(string semesterID)
        {
            var checkSemesterID = await _ISemesterService.GetSemesterById(semesterID);
            if (checkSemesterID == null)
            {
                return NotFound();
            }
            else
            {
                var apiResponse = await _ILecturerCourseGroupService.CreateLecturerCourseGroupInSemester(semesterID);
                if (apiResponse.Success == false)
                {
                    return BadRequest(apiResponse);
                }
                else
                {
                    return Ok(apiResponse);
                }
            }

        }

        [HttpDelete("DeleteLcgInSemester/{semesterID}", Name = "DeleteLecturerCourseGroupInSemester")]
        public async Task<IActionResult> DeleteLecturerCourseGroupInSemester(string semesterID)
        {
            var checkSemesterID = await _ISemesterService.GetSemesterById(semesterID);
            if (checkSemesterID == null)
            {
                return NotFound();
            }
            else
            {
                var apiResponse = await _ILecturerCourseGroupService.DeleteLecturerCourseGroupInSemester(semesterID);
                if (apiResponse.Success == false)
                {
                    return BadRequest(apiResponse);
                }
                else
                {
                    return Ok(apiResponse);
                }
            }

        }

    }
}
