using BEAPICapstoneProjectFLS.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckConstraintController : ControllerBase
    {
        private readonly ICheckConstraintService _ICheckConstraintService;
        private readonly ISemesterService _ISemesterService;

        public CheckConstraintController(ICheckConstraintService CheckConstraintService, ISemesterService SemesterService)
        {
            _ICheckConstraintService = CheckConstraintService;
            _ISemesterService = SemesterService;
        }

        [HttpGet("CheckCourseOflecrurer/{lecturerID}&{semesterID}", Name = "CheckCourseOflecrurer")]
        public async Task<IActionResult> CheckCourseOflecrurer(string lecturerID, string semesterID)
        {
            var checkSemesterID = await _ISemesterService.GetSemesterById(semesterID);
            if (checkSemesterID == null)
            {
                return NotFound();
            }
            else
            {
                var apiResponse = await _ICheckConstraintService.CheckCourseOflecrurer(lecturerID, semesterID);
                if(apiResponse!= null)
                {
                    return Ok(apiResponse);
                }
                return BadRequest();
                //if (apiResponse.Success == false)
                //{
                //    return BadRequest(apiResponse);
                //}
                //else
                //{
                //    return Ok(apiResponse);
                //}
            }

        }


        [HttpGet("CheckSemesterPublic/{semesterID}", Name = "CheckSemesterPublic")]
        public async Task<IActionResult> CheckSemesterPublic(string semesterID)
        {
            var checkSemesterID = await _ISemesterService.GetSemesterById(semesterID);
            if (checkSemesterID == null)
            {
                return NotFound();
            }
            else
            {
                var apiResponse = await _ICheckConstraintService.CheckSemesterPublic(semesterID);
                if (apiResponse != null)
                {
                    return Ok(apiResponse);
                }
                return BadRequest();
            }

        }

        [HttpGet("CheckAllDepartmentManagerConfirm/{semesterID}", Name = "CheckAllDepartmentManagerConfirm")]
        public async Task<IActionResult> CheckAllDepartmentManagerConfirm(string semesterID)
        {
            var checkSemesterID = await _ISemesterService.GetSemesterById(semesterID);
            if (checkSemesterID == null)
            {
                return NotFound();
            }
            else
            {
                var apiResponse = await _ICheckConstraintService.CheckAllDepartmentManagerConfirm(semesterID);
                if (apiResponse != null)
                {
                    return Ok(apiResponse);
                }
                return BadRequest();
            }

        }



        [HttpPut("SetAllDepartmentManagerConfirm/{semesterID}", Name = "SetAllDepartmentManagerConfirm")]
        public async Task<IActionResult> CheckCourseOflecrurer( string semesterID)
        {
            var checkSemesterID = await _ISemesterService.GetSemesterById(semesterID);
            if (checkSemesterID == null)
            {
                return NotFound();
            }
            else
            {
                var apiResponse = await _ICheckConstraintService.SetAllDepartmentManagerConfirm(semesterID);
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
