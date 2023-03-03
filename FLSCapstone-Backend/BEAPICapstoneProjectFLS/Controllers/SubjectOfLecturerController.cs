using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.SubjectOfLecturerRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectOfLecturerController : ControllerBase
    {
        private readonly ISubjectOfLecturerService _ISubjectOfLecturerService;
        private readonly ISemesterService _ISemesterService;

        public SubjectOfLecturerController(ISubjectOfLecturerService SubjectOfLecturerService, ISemesterService SemesterService)
        {
            _ISubjectOfLecturerService = SubjectOfLecturerService;
            _ISemesterService = SemesterService;
        }

        [HttpGet("{id}", Name = "GetSubjectOfLecturerById")]
        public async Task<IActionResult> GetSubjectOfLecturerById(string id)
        {
            var SubjectOfLecturerModel = await _ISubjectOfLecturerService.GetSubjectOfLecturerById(id);
            if (SubjectOfLecturerModel == null)
                return NotFound();
            return Ok(SubjectOfLecturerModel);
        }

        [HttpGet]
        public IActionResult GetAllSubjectOfLecturer([FromQuery] SubjectOfLecturerViewModel flitter, SubjectOfLecturerSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listSubjectOfLecturerModel = _ISubjectOfLecturerService.GetAllSubjectOfLecturer(flitter, pageIndex, pageSize, sortBy, order);
            return Ok(listSubjectOfLecturerModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubjectOfLecturer([FromBody] CreateSubjectOfLecturerRequest request)
        {
            var SubjectOfLecturerVM = await _ISubjectOfLecturerService.CreateSubjectOfLecturer(request);
            if (SubjectOfLecturerVM == null)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute(nameof(GetSubjectOfLecturerById), new { id = SubjectOfLecturerVM.Id }, SubjectOfLecturerVM);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubjectOfLecturer(string id, [FromBody] UpdateSubjectOfLecturerRequest request)
        {
            var SubjectOfLecturerVM = await _ISubjectOfLecturerService.UpdateSubjectOfLecturer(id, request);
            if (SubjectOfLecturerVM == null)
                return BadRequest();
            return Ok(SubjectOfLecturerVM);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubjectOfLecturer(string id)
        {
            var rs = await _ISubjectOfLecturerService.DeleteSubjectOfLecturer(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }

        [HttpPost("CreateSolNewSemester/{semesterID}", Name = "CreateSubjectOfLecturerInSemester")]
        public async Task<IActionResult> CreateSubjectOfLecturerInSemester(string semesterID)
        {
            var checkSemesterID = await _ISemesterService.GetSemesterById(semesterID);
            if (checkSemesterID == null)
            {
                return NotFound();
            }
            else
            {
                var apiResponse = await _ISubjectOfLecturerService.CreateSubjectOfLecturerInSemester(semesterID);
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


        [HttpDelete("DeleteSolInSemester/{semesterID}", Name = "DeleteSubjectOfLecturerInSemester")]
        public async Task<IActionResult> DeleteSubjectOfLecturerInSemester(string semesterID)
        {
            var checkSemesterID = await _ISemesterService.GetSemesterById(semesterID);
            if (checkSemesterID == null)
            {
                return NotFound();
            }
            else
            {
                var apiResponse = await _ISubjectOfLecturerService.DeleteSubjectOfLecturerInSemester(semesterID);
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
