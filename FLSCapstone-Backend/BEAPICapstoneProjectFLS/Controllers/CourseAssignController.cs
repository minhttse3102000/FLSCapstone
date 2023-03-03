using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.CourseAssignRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseAssignController : ControllerBase
    {
        private readonly ICourseAssignService _ICourseAssignService;
        private readonly ISemesterService _ISemesterService;
        private readonly ISubjectService _ISubjectService;

        public CourseAssignController(ICourseAssignService CourseAssignService, ISemesterService SemesterService, ISubjectService SubjectService)
        {
            _ICourseAssignService = CourseAssignService;
            _ISemesterService = SemesterService;
            _ISubjectService = SubjectService;
        }

        [HttpGet("{id}", Name = "GetCourseAssignById")]
        public async Task<IActionResult> GetCourseAssignById(string id)
        {
            var CourseAssignModel = await _ICourseAssignService.GetCourseAssignById(id);
            if (CourseAssignModel == null)
                return NotFound();
            return Ok(CourseAssignModel);
        }
        [HttpGet("CourseAssignByGroup/{GroupID}", Name = "GetCourseAssignByGroup")]
        public async Task<IActionResult> GetAllCourseAssign(string GroupID)
        {
            var listCourseAssignModel = await _ICourseAssignService.GetCourseAssignByGroup(GroupID);
            if (listCourseAssignModel == null)
            {
                return BadRequest();
            }
            return Ok(listCourseAssignModel);
        }

        [HttpGet]
        public IActionResult GetAllCourseAssign([FromQuery] CourseAssignViewModel flitter, CourseAssignSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listCourseAssignModel = _ICourseAssignService.GetAllCourseAssign(flitter, pageIndex, pageSize, sortBy, order);
            return Ok(listCourseAssignModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourseAssign([FromBody] CreateCourseAssignRequest request)
        {
            var CourseAssignVM = await _ICourseAssignService.CreateCourseAssign(request);
            if (CourseAssignVM == null)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute(nameof(GetCourseAssignById), new { id = CourseAssignVM.Id }, CourseAssignVM);
            }
        }

        [HttpPost("AddListCourseAssign/{ScheduleID}", Name = "AddListCourseAssignInSemester")]
        public async Task<IActionResult> CreateListCourseAssign(string ScheduleID, [FromBody] List<CreateCourseAssignRequest> requests)
        {
            var checkCourseAssignVM = await _ICourseAssignService.CreateListCourseAssign(ScheduleID,requests);
            if (checkCourseAssignVM == false)
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourseAssign(string id, [FromBody] UpdateCourseAssignRequest request)
        {
            var CourseAssignVM = await _ICourseAssignService.UpdateCourseAssign(id, request);
            if (CourseAssignVM == null)
                return BadRequest();
            return Ok(CourseAssignVM);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourseAssign(string id)
        {
            var rs = await _ICourseAssignService.DeleteCourseAssign(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }


        [HttpDelete("DeleteListCourseAssign/{ScheduleID}")]
        public async Task<IActionResult> DeleteListCourseAssignInSemester(string ScheduleID)
        {
            var rs = await _ICourseAssignService.DeleteListCourseAssignInSemester(ScheduleID);
            if (rs == false)
                return NotFound();
            return Ok();
        }

        [HttpDelete("DeleteAssignedCourses/{ScheduleID}")]
        public async Task<IActionResult> DeleteAssignedCourses(string ScheduleID)
        {
            var rs = await _ICourseAssignService.DeleteAssignedCourses(ScheduleID);
            if (rs == false)
                return NotFound();
            return Ok();
        }


        [HttpGet("GetUserAssignInDepartment/{subjectID}&{semesterID}", Name = "GetUserAssignInDepartment")]
        public async Task<IActionResult> GetUserAssignInDepartment(string subjectID, string semesterID)
        {
            var checkSemesterID = await _ISemesterService.GetSemesterById(semesterID);
            var checkSubjectID = await _ISubjectService.GetSubjectById(subjectID);

            if (checkSemesterID == null || checkSubjectID == null)
            {
                return NotFound();
            }
            else
            {
                var rs = await _ICourseAssignService.GetUserAssignInDepartment(subjectID, semesterID);
                if (rs == null)
                    return NotFound();
                return Ok(rs);
            }

        }

        [HttpGet("GetUserAssignOutDepartment/{subjectID}&{semesterID}", Name = "GetUserAssignOutDepartment")]
        public async Task<IActionResult> GetUserAssignOutDepartment(string subjectID, string semesterID)
        {
            var checkSemesterID = await _ISemesterService.GetSemesterById(semesterID);
            var checkSubjectID = await _ISubjectService.GetSubjectById(subjectID);

            if (checkSemesterID == null || checkSubjectID == null)
            {
                return NotFound();
            }
            else
            {
                var rs = await _ICourseAssignService.GetUserAssignOutDepartment(subjectID, semesterID);
                if (rs == null)
                    return NotFound();
                return Ok(rs);
            }


            
        }
    }
}
