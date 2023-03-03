using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.ViewModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BEAPICapstoneProjectFLS.Requests.SemesterRequest;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _ISemesterService;

        public SemesterController(ISemesterService SemesterService)
        {
            _ISemesterService = SemesterService;
        }

        [HttpGet("{id}", Name = "GetSemesterById")]
        public async Task<IActionResult> GetSemesterById(string id)
        {
            var SemesterModel = await _ISemesterService.GetSemesterById(id);
            if (SemesterModel == null)
                return NotFound();
            return Ok(SemesterModel);
        }

        [HttpGet]
        public IActionResult GetAllSemester([FromQuery] SemesterViewModel flitter, SemesterSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listSemesterModel = _ISemesterService.GetAllSemester(flitter, pageIndex, pageSize,
                sortBy, order);
            return Ok(listSemesterModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSemester([FromBody] CreateSemesterRequest request)
        {
            var SemesterVM = await _ISemesterService.CreateSemester(request);
            if (SemesterVM == null)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute(nameof(GetSemesterById), new { id = SemesterVM.Id }, SemesterVM);
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSemester(string id, [FromBody] UpdateSemesterRequest request)
        {
            var SemesterModelFindId = await _ISemesterService.GetSemesterById(id);
            if (SemesterModelFindId == null)
                return NotFound();
            else
            {
                var SemesterVM = await _ISemesterService.UpdateSemester(id, request);
                if (SemesterVM == null)
                    return BadRequest();
                return Ok(SemesterVM);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemester(string id)
        {
            var rs = await _ISemesterService.DeleteSemester(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }
    }
}
