using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.RoomTypeRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeService _IRoomTypeService;

        public RoomTypeController(IRoomTypeService RoomTypeService)
        {
            _IRoomTypeService = RoomTypeService;
        }

        [HttpGet("{id}", Name = "GetRoomTypeById")]
        public async Task<IActionResult> GetRoomTypeById(string id)
        {
            var RoomTypeModel = await _IRoomTypeService.GetRoomTypeById(id);
            if (RoomTypeModel == null)
                return NotFound();
            return Ok(RoomTypeModel);
        }

        [HttpGet]
        public IActionResult GetAllRoomType([FromQuery] RoomTypeViewModel flitter, RoomTypeSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listRoomTypeModel = _IRoomTypeService.GetAllRoomType(flitter, pageIndex, pageSize, sortBy, order);
            return Ok(listRoomTypeModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoomType([FromBody] CreateRoomTypeRequest request)
        {
            var RoomTypeVM = await _IRoomTypeService.CreateRoomType(request);
            if (RoomTypeVM == null)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute(nameof(GetRoomTypeById), new { id = RoomTypeVM.Id }, RoomTypeVM);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomType(string id, [FromBody] UpdateRoomTypeRequest request)
        {
            var RoomTypeVM = await _IRoomTypeService.UpdateRoomType(id, request);
            if (RoomTypeVM == null)
                return BadRequest();
            return Ok(RoomTypeVM);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomType(string id)
        {
            var rs = await _IRoomTypeService.DeleteRoomType(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }
    }
}
