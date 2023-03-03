using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.RoomSemesterRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomSemesterController : ControllerBase
    {
        private readonly IRoomSemesterService _IRoomSemesterService;

        public RoomSemesterController(IRoomSemesterService RoomSemesterService)
        {
            _IRoomSemesterService = RoomSemesterService;
        }

        [HttpGet("{id}", Name = "GetRoomSemesterById")]
        public async Task<IActionResult> GetRoomSemesterById(string id)
        {
            var RoomSemesterModel = await _IRoomSemesterService.GetRoomSemesterById(id);
            if (RoomSemesterModel == null)
                return NotFound();
            return Ok(RoomSemesterModel);
        }

        [HttpGet]
        public IActionResult GetAllRoomSemester([FromQuery] RoomSemesterViewModel flitter, RoomSemesterSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listRoomSemesterModel = _IRoomSemesterService.GetAllRoomSemester(flitter, pageIndex, pageSize, sortBy, order);
            return Ok(listRoomSemesterModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoomSemester([FromBody] CreateRoomSemesterRequest request)
        {
            var RoomSemesterVM = await _IRoomSemesterService.CreateRoomSemester(request);
            if (RoomSemesterVM == null)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute(nameof(GetRoomSemesterById), new { id = RoomSemesterVM.Id }, RoomSemesterVM);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomSemester(string id, [FromBody] UpdateRoomSemesterRequest request)
        {
            var RoomSemesterVM = await _IRoomSemesterService.UpdateRoomSemester(id, request);
            if (RoomSemesterVM == null)
                return BadRequest();
            return Ok(RoomSemesterVM);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomSemester(string id)
        {
            var rs = await _IRoomSemesterService.DeleteRoomSemester(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }
    }
}
