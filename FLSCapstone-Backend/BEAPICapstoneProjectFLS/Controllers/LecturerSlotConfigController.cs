using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.LecturerSlotConfigRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerSlotConfigController : ControllerBase
    {
        private readonly ILecturerSlotConfigService _ILecturerSlotConfigService;
        private readonly ISemesterService _ISemesterService;

        public LecturerSlotConfigController(ILecturerSlotConfigService LecturerSlotConfigService, ISemesterService SemesterService)
        {
            _ILecturerSlotConfigService = LecturerSlotConfigService;
            _ISemesterService = SemesterService;

        }

        [HttpGet("{id}", Name = "GetLecturerSlotConfigById")]
        public async Task<IActionResult> GetLecturerSlotConfigById(string id)
        {
            var LecturerSlotConfigModel = await _ILecturerSlotConfigService.GetLecturerSlotConfigById(id);
            if (LecturerSlotConfigModel == null)
                return NotFound();
            return Ok(LecturerSlotConfigModel);
        }

        [HttpGet]
        public IActionResult GetAllLecturerSlotConfig([FromQuery] LecturerSlotConfigViewModel flitter, LecturerSlotConfigSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listLecturerSlotConfigModel = _ILecturerSlotConfigService.GetAllLecturerSlotConfig(flitter, pageIndex, pageSize, sortBy, order);
            return Ok(listLecturerSlotConfigModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLecturerSlotConfig([FromBody] CreateLecturerSlotConfigRequest request)
        {
            var LecturerSlotConfigVM = await _ILecturerSlotConfigService.CreateLecturerSlotConfig(request);
            if (LecturerSlotConfigVM == null)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute(nameof(GetLecturerSlotConfigById), new { id = LecturerSlotConfigVM.Id }, LecturerSlotConfigVM);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLecturerSlotConfig(string id, [FromBody] UpdateLecturerSlotConfigRequest request)
        {
            var LecturerSlotConfigVM = await _ILecturerSlotConfigService.UpdateLecturerSlotConfig(id, request);
            if (LecturerSlotConfigVM == null)
                return BadRequest();
            return Ok(LecturerSlotConfigVM);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLecturerSlotConfig(string id)
        {
            var rs = await _ILecturerSlotConfigService.DeleteLecturerSlotConfig(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }


        [HttpPost("CreateStAndLscNewSemester/{semesterID}", Name = "CreateSlotTypesAndLecturerSlotConfigsInSemester")]
        public async Task<IActionResult> CreateSlotTypesAndLecturerSlotConfigsInSemester(string semesterID)
        {
            var checkSemesterID = await _ISemesterService.GetSemesterById(semesterID);
            if (checkSemesterID == null)
            {
                return NotFound();
            }
            else
            {
                var apiResponse = await _ILecturerSlotConfigService.CreateSlotTypesAndLecturerSlotConfigsInSemester(semesterID);
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

        [HttpDelete("DeleteLscInSemester/{semesterID}", Name = "DeleteLecturerSlotConfigInSemester")]
        public async Task<IActionResult> DeleteLecturerSlotConfigInSemester(string semesterID)
        {
            var checkSemesterID = await _ISemesterService.GetSemesterById(semesterID);
            if (checkSemesterID == null)
            {
                return NotFound();
            }
            else
            {
                var apiResponse = await _ILecturerSlotConfigService.DeleteLecturerSlotConfigInSemester(semesterID);
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
